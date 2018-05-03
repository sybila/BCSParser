using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.DAL.Api
{
    public class ApiAnnotation : IEntity<int>
    {
        public int Id { get; set; }
        public string TermId { get; set; }
        public string TermType { get; set; }
    }
}