using BcsResolver.SemanticModel;
using System;
using System.Collections.Generic;
using System.Text;
using BcsResolver.File;
using BcsAdmin.DAL.Models;
using System.Linq;

namespace BcsAdmin.BL.Services
{
    public class DbBcsEntityMetadataProvider : IBcsEntityMetadataProvider
    {
        private readonly EcyanoNewDbContext dbContext;

        public DbBcsEntityMetadataProvider(EcyanoNewDbContext ecyanoNewDbContext)
        {
            this.dbContext = ecyanoNewDbContext;
        }

        public IEnumerable<string> GetAvailableEntityIds()
        {
            return dbContext.EpEntity
                .Select(e => e.Name);
        }

        public BcsEntity GetEntity(string entityId)
        {
            var dbEntity = dbContext.EpEntity.SingleOrDefault(e => e.Code == entityId);

            var newEntity = new BcsEntity
            {
                Id = dbEntity.Name,
                Name = dbEntity.Code,
                Type = (BcsEntityType)Enum.Parse(typeof(BcsEntityType), dbEntity.Type,true),
            };

            newEntity.States.AddRange(dbEntity.Children.Where(e => e.Type == "state").Select(e => e.Code));
            newEntity.Composition.AddRange(dbEntity.Children.Where(e => e.Type == "entity").Select(e => e.Code));
            //newEntity.Locations.AddRange(dbEntity.Locations.Select(e => e.Parent?.Code).Where(e=>e != null));

            return newEntity;
        }
    }
}
