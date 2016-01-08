using BcsResolver.Extensions;
using BcsResolver.Parser;
using BcsResolver.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.File
{
    public class BcsFileHandler : IDisposable
    {
        public BcsDefinitionFile DefinitionFile { get; private set; }
        public BcsExpresionTokenizer Tokenizer { get; private set; } = new BcsExpresionTokenizer();
        public BcsParser Parser { get; private set; } = new BcsParser();
        public BcsDefinitionFileReader FileReader { get; private set; } = new BcsDefinitionFileReader();

        public void ProcessDefinitionFile(string fileName)
        {
            DefinitionFile = FileReader.ReadFile(fileName);

            ProcessEntities();

            ProcessRules();
        }

        private void ProcessRules()
        {
            ParseReactions();

            ParseModifiers();

            PostProcessExpressionTrees();
        }

        private void ProcessEntities()
        {
            CreateLocations();

            CreateAtomicAgents();

            CreateStates();

            CreateComponents();

            CreateComplexes();
        }

        private void CreateStates()
        {
            var states = DefinitionFile.AtomicAgents.SelectMany(a => a.AllStates.Select(s => s.Name)).Distinct();

            DefinitionFile.States.AddRange(states.Select(s => new BcsAgentStateNode { Name = s }));
        }

        private void CreateLocations()
        {
            var locations = DefinitionFile.Entities.SelectMany(entity => entity.Locations).Distinct();
            var nodes = locations.Select(l => new BcsLocationNode { Name = l });
            DefinitionFile.Locations.AddRange(nodes);
        }

        private void CreateAtomicAgents()
        {
            var atomicAgents = DefinitionFile.Entities.Where(e => !e.States.SourceText.IsEmptyOrWhitespace());

            foreach (var agetEntity in atomicAgents)
            {
                var states = agetEntity.States.SourceText.Split(',')
                    .Select(s => s.Trim());

                var agent = new BcsAtomicAgentNode()
                {
                    Name = agetEntity.Id
                };
                agent.AllStates.AddRange(states.Select(s => new BcsAgentStateNode { Name = s }));

                DefinitionFile.AtomicAgents.Add(agent);
            }
        }

        private void CreateComponents()
        {
            var components = DefinitionFile.Entities.Where(e => e.States.SourceText.IsEmptyOrWhitespace() && !e.Composition.SourceText.Contains("."));

            foreach (var component in components)
            {
                var componentNode = new BcsComponentNode()
                {
                    Name = component.Id
                };

                if (!component.Composition.SourceText.IsEmptyOrWhitespace())
                {
                    var componentAgents = component.Composition.SourceText.Split('|').Select(name =>
                    {
                        name = name.Trim();

                        var matchingAgent = DefinitionFile.AtomicAgents.SingleOrDefault(agent => agent.Name == name);
                        if (matchingAgent == null)
                        {
                            componentNode.Errors.Add(new NodeError($"Failed to find matching agent named {name} in the definition file."));
                            var dummyAgent = new BcsAtomicAgentNode() { Name = name };
                            DefinitionFile.AtomicAgents.Add(dummyAgent);
                            return dummyAgent;
                        }
                        return matchingAgent;
                    }).ToList();

                    componentNode.AtomicAgents.AddRange(componentAgents);
                }

                DefinitionFile.Components.Add(componentNode);
            }
        }

        public void CreateComplexes()
        {
            var complexes = DefinitionFile.Entities.Where(e => e.States.SourceText.IsEmptyOrWhitespace() && e.Composition.SourceText.Contains("."));

            foreach (var complex in complexes)
            {
                var complexNode = new BcsComplexNode() { Name = complex.Name };

                var compoennts = complex.Composition.SourceText.Split('.').Select(name =>
                {
                    name = name.Trim();

                    var matchingComponent = DefinitionFile.Components.SingleOrDefault(component => component.Name == name);
                    if (matchingComponent == null)
                    {
                        complexNode.Errors.Add(new NodeError($"Failed to find matching component named {name} in the definition file."));
                        var dummyComponent = new BcsComponentNode() { Name = name };
                        DefinitionFile.Components.Add(dummyComponent);
                        return dummyComponent;
                    }
                    return matchingComponent;
                }).ToList();

                complexNode.Components.AddRange(compoennts);
                DefinitionFile.Complexes.Add(complexNode);
            }
        }

        private void ParseModifiers()
        {
            var modifiers = DefinitionFile.Rules.Select(r => r.Modifier);

            foreach (var modifierAdapter in modifiers)
            {
                Tokenizer.Tokenize(new Common.StringReader(modifierAdapter.SourceText));
                modifierAdapter.SourceTokens.AddRange(Tokenizer.Tokens);

                modifierAdapter.ExpressionNode = Parser.ParseComplex(modifierAdapter.SourceTokens);
            }
        }

        public void ParseReactions()
        {
            var equations = DefinitionFile.Rules.Select(r => r.Equation);

            foreach (var equationAdapter in equations)
            {
                Tokenizer.Tokenize(new Common.StringReader(equationAdapter.SourceText));
                equationAdapter.SourceTokens.AddRange(Tokenizer.Tokens);

                equationAdapter.ExpressionNode = Parser.ParseReaction(equationAdapter.SourceTokens);
            }
        }

        private void PostProcessExpressionTrees()
        {
            foreach (var equationNode in DefinitionFile.Rules.Select(r => r.Equation.ExpressionNode).Where(en => en != null))
            {
                var visitor = new SemanticAnalysisVisitor { DefinitionFile = DefinitionFile };

                visitor.Visit(equationNode);
            }

            foreach (var modifierNode in DefinitionFile.Rules.Select(r => r.Modifier.ExpressionNode).Where(en => en != null))
            {
                var visitor = new SemanticAnalysisVisitor { DefinitionFile = DefinitionFile };

                visitor.Visit(modifierNode);
            }
        }

        public void Dispose()
        {
            FileReader.Dispose();
        }
    }
}
