using BcsAdmin.BL.Services;
using BcsAdmin.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BcsAdmin.Tests
{
    [TestClass]
    public class DbProviderSmokeTests
    {
        [TestMethod]
        public void EntityProvider_GetAvailableEntityIds()
        {
            using (var c = new EcyanoNewDbContext())
            {
                var provider = new DbBcsEntityMetadataProvider(c);

                var es = provider.GetAvailableEntityIds().ToList();
            }
        }

        [TestMethod]
        public void EntityProvider_GetEntity()
        {
            using (var w = new DbWorkspace(new EcyanoNewDbContext()))  {
                w.CreateSemanticModel();
             }
        }

    }
}
