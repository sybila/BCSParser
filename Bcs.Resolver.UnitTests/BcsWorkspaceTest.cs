﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BcsResolver.File;
using BcsResolver.SemanticModel;
using BcsResolver.SemanticModel.Tree;
using BcsResolver.Tests.Helpers;
using Moq;
using System.Threading;
using System.Threading.Tasks;

namespace BcsResolver.Tests
{
    [TestClass]
    public class BcsWorkspaceTest
    {
        private static Dictionary<string, BcsEntity> entityPool = TestCaseFactory.CreateValidEntityPool();

        [TestMethod]
        public async Task SemanticModel_EmptyComplex_ValidAsync()
        {
            var mock = new Mock<IBcsEntityMetadataProvider>();

            mock.Setup(p => p.GetEntity(It.IsAny<string>())).Returns(() => entityPool["cx1"]);
            mock.Setup(p => p.GetAvailableEntityIds()).Returns(() => new[] { "cx1" });


            var workspace = new BcsFileWorkspace(mock.Object);

            await workspace.CreateSemanticModelAsync(CancellationToken.None);

            var complex = workspace.Complexes.First().Value;
            Assert.AreEqual(complex.Name, "cx1");
            Assert.AreEqual(complex.Type, BcsSymbolType.Complex);
            Assert.AreEqual(complex.Locations.Count, 2);
            Assert.AreEqual(complex.Locations[0].Name, "l1");
            Assert.AreEqual(complex.Locations[1].Name, "l2");
        }

        [TestMethod]
        public void SemanticModel_EmptyComponent_Valid()
        {
            var mock = new Mock<IBcsEntityMetadataProvider>();

            mock.Setup(p => p.GetEntity(It.IsAny<string>())).Returns(() => entityPool["ct1"]);
            mock.Setup(p => p.GetAvailableEntityIds()).Returns(() => new[] { "ct1" });


            var workspace = new BcsFileWorkspace(mock.Object);

            workspace.CreateSemanticModelAsync(CancellationToken.None);

            var componentSymbol = workspace.StructuralAgents.First().Value;
            Assert.AreEqual(componentSymbol.Name, "ct1");
            Assert.AreEqual(componentSymbol.Type, BcsSymbolType.StructuralAgent);
            Assert.AreEqual(componentSymbol.Locations.Count, 2);
            Assert.AreEqual(componentSymbol.Locations[0].Name, "l1");
            Assert.AreEqual(componentSymbol.Locations[1].Name, "l2");
        }

        [TestMethod]
        public async Task SemanticModel_AgentWithStates_ValidAsync()
        {
            var mock = new Mock<IBcsEntityMetadataProvider>();

            mock.Setup(p => p.GetEntity(It.IsAny<string>())).Returns(() => entityPool["ag1"]);
            mock.Setup(p => p.GetAvailableEntityIds()).Returns(() => new[] { "ag1" });


            var workspace = new BcsFileWorkspace(mock.Object);

            await workspace.CreateSemanticModelAsync(CancellationToken.None);

            var agentSymbol = workspace.AtomicAgents.First().Value;
            Assert.AreEqual("ag1", agentSymbol.Name);
            Assert.AreEqual(BcsSymbolType.Agent, agentSymbol.Type);
            Assert.AreEqual(agentSymbol.Locations.Count, 2);
            Assert.AreEqual("l1", agentSymbol.Locations[0].Name);
            Assert.AreEqual("l2", agentSymbol.Locations[1].Name);
            Assert.AreEqual(3, agentSymbol.States.Count());
            Assert.IsInstanceOfType(agentSymbol.States.ElementAt(0), typeof(BcsStateSymbol));
            Assert.IsInstanceOfType(agentSymbol.States.ElementAt(1), typeof(BcsStateSymbol));
            Assert.IsInstanceOfType(agentSymbol.States.ElementAt(2), typeof(BcsStateSymbol));
        }

        [TestMethod]
        public async Task SemanticModel_FullComplex_ValidAsync()
        {
            var mock = new Mock<IBcsEntityMetadataProvider>();

            mock.Setup(p => p.GetEntity(It.IsAny<string>())).Returns<string>(e => entityPool[e]);
            mock.Setup(p => p.GetAvailableEntityIds()).Returns(() => new[] { "ag2", "ag3", "ct2", "ct3", "cx2" });


            var workspace = new BcsFileWorkspace(mock.Object);

            await workspace.CreateSemanticModelAsync(CancellationToken.None);

            var complexSymbol = workspace.Complexes.First().Value;
            var cComponents = complexSymbol.StructuralAgents.AssertCount(2);
            Assert.AreEqual("ct2", cComponents.ElementAt(0).Name);
            Assert.AreEqual("ct3", cComponents.ElementAt(1).Name);

            var firstAgents = cComponents.ElementAt(0).AtomicAgents.AssertCount(2);
            var secondAtgents = cComponents.ElementAt(1).AtomicAgents.AssertCount(1);

            var firsrFirstAgent = firstAgents.ElementAt(0);
            var firsrSecondAgent = firstAgents.ElementAt(1);

            var secondFirstAgent = secondAtgents.ElementAt(0);

            Assert.AreEqual("ag1", firsrFirstAgent.Name);
            Assert.AreEqual("ag2", firsrSecondAgent.Name);

            Assert.AreEqual("ag3", secondFirstAgent.Name);

            var firsrFirstAgentStates = firsrFirstAgent.States.AssertCount(3);
            var firsrSecondAgentStates = firsrSecondAgent.States.AssertCount(2);
            var secondFirstAgentStates = secondFirstAgent.States.AssertCount(2);

            Assert.AreEqual("a", firsrFirstAgentStates.ElementAt(0).Name);
            Assert.AreEqual("a", firsrSecondAgentStates.ElementAt(0).Name);
            Assert.AreEqual("p", secondFirstAgentStates.ElementAt(0).Name);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task SemanticModel_UndefinedAgent_ErrorAsync()
        {
            var mock = new Mock<IBcsEntityMetadataProvider>();

            mock.Setup(p => p.GetEntity(It.IsAny<string>())).Returns<string>(e =>
            {
                try
                {
                    return entityPool[e];
                }
                catch
                {
                    return null;
                }
            });
            mock.Setup(p => p.GetAvailableEntityIds()).Returns(() => new[] { "ctE" });

            var workspace = new BcsFileWorkspace(mock.Object);
            await workspace.CreateSemanticModelAsync(CancellationToken.None);

            var componentSymbol = workspace.StructuralAgents.AssertCount(1).First().Value;
            var errorSymbol = componentSymbol.Parts.AssertCount(1).First().AssertCast<BcsErrorSymbol>();
            Assert.AreEqual("Entity not found: Entity agUndefined is not defined.", errorSymbol.Error);
            Assert.AreEqual(BcsSymbolType.Agent, errorSymbol.ExpectedType);
        }
    }
}
