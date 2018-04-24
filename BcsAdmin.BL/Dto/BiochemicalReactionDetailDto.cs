using Riganti.Utils.Infrastructure.Core;

namespace Bcs.Admin.BL.Dto
{
    public class BiochemicalReactionDetailDto : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Equation { get; set; }
        public string Modifier { get; set; }
        public int? Active { get; set; }
        public int? IsValid { get; set; }
    }
}
