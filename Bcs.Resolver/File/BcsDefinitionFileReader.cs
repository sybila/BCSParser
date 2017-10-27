using BcsResolver.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BcsResolver.SemanticModel;

namespace BcsResolver.File
{
    public class BcsDefinitionFileReader : IDisposable
    {
        private BcsDefinitionFile definitionFile;

        private StreamReader reader;

        private string currentLine;

        public BcsDefinitionFile ReadFile(string filename)
        {
            reader = new StreamReader(filename, true);

            Clear();

            while (!reader.EndOfStream)
            {
                SkipEmptyLines();

                ReadRecord();
            }

            return definitionFile;
        }

        private void Clear()
        {
            definitionFile = new BcsDefinitionFile();
            currentLine = string.Empty;
        }

        private void ReadRecord()
        {
            if (IsAtEntityStart())
            {
                var record = new BcsEntity();
                ReadRecordProperties(record, AssignEntityProperties);
                definitionFile.Entities.Add(record);
            }
            else if (IsAtRuleStart())
            {
                var record = new BcsRule();
                ReadRecordProperties(record, AssignRuleProperties );
                definitionFile.Rules.Add(record);
            }
            else
            {
                ReadArtifact();
            }
        }

        private void ReadArtifact()
        {
            while (!reader.EndOfStream && !currentLine.IsEmptyOrWhitespace())
            {
                if (IsAtEntityStart() || IsAtRuleStart())
                {
                    break;
                }
                definitionFile.Artifacts.Add(currentLine);
                NextLine();
            }
        }

        private void ReadRecordProperties(BcsFileRecord record, Func<BcsFileRecord, string, string, bool> propertyProcessingFunc )
        {
            while (!reader.EndOfStream && !currentLine.IsEmptyOrWhitespace())
            {
                var propertySeparatorIndex = currentLine.IndexOf(':');

                if (propertySeparatorIndex < 0)
                {
                    record.MalformedLines.Add(currentLine);
                }
                else
                {
                    string name = currentLine.Substring(0, propertySeparatorIndex).RemoveAllWhitespaces();
                    string value = string.Empty;

                    if (propertySeparatorIndex + 1 < currentLine.Length)
                    {
                        value = currentLine.Substring(propertySeparatorIndex+1).Trim();
                    }

                    if(!propertyProcessingFunc(record, name, value) && !AssignCommonRecordProperties(name, value, record) )
                    {
                        record.MalformedLines.Add(currentLine);
                    }
                }

                NextLine();
            }

        }

        private bool AssignEntityProperties(BcsFileRecord record, string name, string value)
        {
            var entity = record as BcsEntity;

            if (name.CaselessEquals("entityid"))
            {
                entity.Id = value;
            }
            else if (name.CaselessEquals("states"))
            {
                var stateNames = SplitNames(value, ',');
                entity.States.AddRange(stateNames);
                if (stateNames.Any())
                {
                    entity.Type = BcsEntityType.Agent;
                }
            }
            else if (name.CaselessEquals("locations"))
            {
                var names = SplitNames(value, ',');
                entity.Locations.AddRange(names);
            }
            else if (name.CaselessEquals("composition"))
            {
                char separator = '\n';
                if (value.Contains("."))
                {
                    separator = '.';
                    entity.Type = BcsEntityType.Complex;
                }
                else if (value.Contains(","))
                {
                    separator = ',';
                    entity.Type = BcsEntityType.Component;
                }
                entity.Composition.AddRange(SplitNames(value,separator));
            }
            else if (name.CaselessEquals("entityname"))
            {
                entity.Name = value;
            }
            else
            {
                return false;
            }
            return true;
        }

        private static string[] SplitNames(string value, char separator)
        {
            return value.Split(separator).Select(s => s.Trim()).ToArray();
        }

        private bool AssignRuleProperties(BcsFileRecord record, string name, string value)
        {
            var rule = record as BcsRule;

            if (name.CaselessEquals("ruleid"))
            {
                rule.Id = value;
            }
            else if (name.CaselessEquals("ruleequation"))
            {
                rule.Equation = value;
            }
            else if (name.CaselessEquals("modifier"))
            {
                rule.Modifier = value;
            }
            else if (name.CaselessEquals("rulename"))
            {
                rule.Name = value;
            }
            else
            {
                return false;
            }
            return true;
        }

        private bool AssignCommonRecordProperties(string name, string value, BcsFileRecord record)
        {
            if(name.CaselessEquals("organism"))
            {
                record.Organism = value;
            }
            else if (name.CaselessEquals("classification"))
            {
                record.Classification = value;
            }
            else if (name.CaselessEquals("description"))
            {
                record.Description = value;
            }
            else if (name.CaselessEquals("links"))
            {
                record.Links = value;
            }
            else if (name.CaselessEquals("notes"))
            {
                record.Notes = value;
            }
            else
            {
                return false;
            }
            return true;
        }

        private bool IsAtRuleStart()
        {
            return currentLine.StartsWith("rule", StringComparison.InvariantCultureIgnoreCase);
        }

        private bool IsAtEntityStart()
        {
            return currentLine.StartsWith("entity", StringComparison.InvariantCultureIgnoreCase);
        }

        private void SkipEmptyLines()
        {
            while (!reader.EndOfStream)
            {
                if (!currentLine.IsEmptyOrWhitespace()) { break; }

                NextLine();
            }
        }

        private void NextLine()
        {
            currentLine = reader.ReadLine();
        }

        public void Dispose()
        {
            reader?.Dispose();
        }
    }
}
