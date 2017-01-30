using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Extensions;
using BcsResolver.SemanticModel;

namespace BcsResolver.File
{
    public class BcsFileMetadataProvider : IBcsEntityMetadataProvider
    {
        private readonly BcsDefinitionFile _file;

        public BcsFileMetadataProvider(BcsDefinitionFile file)
        {
            _file = file;
        }

        public IEnumerable<string> GetAvailableEntityIds()
        {
            return _file.Entities.Select(e => e.Id).ToList();
        }

        public BcsEntity GetEntity(string entityId) =>
            _file.Entities
                .SingleOrDefault(e=> e.Id== entityId);


    }
}
