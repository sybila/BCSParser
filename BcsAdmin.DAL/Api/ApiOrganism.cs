using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.DAL.Api
{
    public class ApiOrganism : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
