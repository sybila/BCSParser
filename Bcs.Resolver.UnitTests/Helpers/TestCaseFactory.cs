using System.Collections.Generic;
using System.Linq;
using BcsResolver.File;
using BcsResolver.SemanticModel.Tree;
using BcsResolver.Syntax.Parser;
using Castle.Components.DictionaryAdapter;

namespace BcsResolver.Tests.Helpers
{
    public static class TestCaseFactory
    {
        public static Dictionary<string, BcsEntity> CreateValidEntityPool() =>
            new Dictionary<string, BcsEntity>
            {
                {
                    "cx1",
                    new BcsEntity
                    {
                        Id = "cx1",
                        Type = BcsEntityType.Complex,
                        Locations = { "l1", "l2" },
                        Composition = { },
                        States = { }
                    }
                },
                {
                    "ct1",
                    new BcsEntity
                    {
                        Id = "ct1",
                        Type = BcsEntityType.Component,
                        Locations = { "l1", "l2" },
                        Composition = { },
                        States = { }
                    }
                },
                {
                    "ag1",
                    new BcsEntity
                    {
                        Id = "ag1",
                        Type = BcsEntityType.Agent,
                        Locations = { "l1", "l2" },
                        Composition = { },
                        States = { "a","b","c"}
                    }
                },
                {
                    "ag2",
                    new BcsEntity
                    {
                        Id = "ag2",
                        Type = BcsEntityType.Agent,
                        Locations = { "l1", "l3" },
                        Composition = { },
                        States = { "a","c"}
                    }
                },
                {
                    "ag3",
                    new BcsEntity
                    {
                        Id = "ag3",
                        Type = BcsEntityType.Agent,
                        Locations = { "l1", "l3" },
                        Composition = { },
                        States = { "p","s"}
                    }
                },
                {
                    "ct2",
                    new BcsEntity
                    {
                        Id = "ct2",
                        Type = BcsEntityType.Component,
                        Locations = { "l1", "l4" },
                        Composition = { "ag1", "ag2" },
                        States = { }
                    }
                },
                {
                    "ct3",
                    new BcsEntity
                    {
                        Id = "ct3",
                        Type = BcsEntityType.Component,
                        Locations = { "l1", "l3" },
                        Composition = { "ag3" },
                        States = { }
                    }
                },
                {
                    "cx2",
                    new BcsEntity
                    {
                        Id = "cx2",
                        Type = BcsEntityType.Complex,
                        Locations = { "l1", "l2" },
                        Composition = { "ct2","ct3" },
                        States = { }
                    }
                },
                {
                    "ctE",
                    new BcsEntity
                    {
                        Id = "ctE",
                        Type = BcsEntityType.Component,
                        Locations = { "l1", "l2" },
                        Composition = { "agUndefined" },
                        States = { }
                    }
                },
            };

        public static BcsComplexSymbol CreateThreePartComplex(List<BcsCompartmentSymbol> withLocations = null)
        {
            var namedLocationSymbols = withLocations?.OfType<BcsNamedSymbol>().ToList() ?? new List<BcsNamedSymbol>();

            return new BcsComplexSymbol
            {
                Parts = new List<BcsNamedSymbol>()
                {
                    new BcsStructuralAgentSymbol
                    {
                        Name = "A",
                        Locations = namedLocationSymbols,
                        Parts = new List<BcsNamedSymbol>
                        {
                            new BcsAtomicAgentSymbol
                            {
                                Name = "B",
                                Parts = new List<BcsNamedSymbol>
                                {
                                    new BcsStateSymbol {Name = "u"},
                                    new BcsStateSymbol {Name = "p"},
                                    new BcsStateSymbol {Name = "q"}
                                }
                            },
                             new BcsAtomicAgentSymbol
                            {
                                Name = "G",
                                Parts = new List<BcsNamedSymbol>
                                {
                                    new BcsStateSymbol {Name = "u"},
                                    new BcsStateSymbol {Name = "q"}
                                }
                            }
                        }
                    },
                    new BcsStructuralAgentSymbol
                    {
                        Name = "C",
                        Locations = namedLocationSymbols,
                        Parts = new List<BcsNamedSymbol>
                        {
                            new BcsAtomicAgentSymbol
                            {
                                Name = "D",
                                Parts = new List<BcsNamedSymbol>
                                {
                                    new BcsStateSymbol {Name = "p"},
                                    new BcsStateSymbol {Name = "r"}
                                }
                            }
                        }
                    },
                    new BcsStructuralAgentSymbol
                    {
                        Name = "E",
                        Locations =namedLocationSymbols,
                        Parts = new List<BcsNamedSymbol>
                        {
                            new BcsAtomicAgentSymbol
                            {
                                Name = "F",
                                Parts = new List<BcsNamedSymbol>
                                {
                                    new BcsStateSymbol {Name = "i"},
                                    new BcsStateSymbol {Name = "j"}
                                }
                            }
                        }
                    }
                },
                Locations = namedLocationSymbols
            };
        }

        public static BcsComplexSymbol CreateMixedAgentComplex()
        {
            var cyt = new BcsCompartmentSymbol { Name = "cyt" };
            return new BcsComplexSymbol
            {
                Name = "mixed",
                Locations = new List<BcsNamedSymbol> { cyt },
                Parts = new List<BcsNamedSymbol>
                {
                    new BcsAtomicAgentSymbol
                    {
                        Name = "Tyr",
                        Parts = new List<BcsNamedSymbol>
                                {
                                    new BcsStateSymbol {Name = "u"},
                                    new BcsStateSymbol {Name = "v"}
                                },
                        Locations = new List<BcsNamedSymbol> {cyt}
                    },
                    new BcsStructuralAgentSymbol
                    {
                        Name = "FRS",
                        Locations = new List<BcsNamedSymbol> {cyt},
                        Parts = new List<BcsNamedSymbol>
                        {
                            new BcsAtomicAgentSymbol
                            {
                                Name ="Thr",
                                Parts = new List<BcsNamedSymbol>
                                {
                                    new BcsStateSymbol {Name = "p"},
                                    new BcsStateSymbol {Name = "r"}
                                },
                                Locations = new List<BcsNamedSymbol> {cyt}
                            }
                        }

                    }
                }
            };
        }
    }
}