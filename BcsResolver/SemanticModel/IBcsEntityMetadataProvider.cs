using System.Collections.Generic;
using BcsResolver.File;

namespace BcsResolver.SemanticModel
{
    public interface IBcsEntityMetadataProvider
    {
        BcsEntity GetEntity(string entityId);
        IEnumerable<string> GetAvailableEntityIds();
    }
}
