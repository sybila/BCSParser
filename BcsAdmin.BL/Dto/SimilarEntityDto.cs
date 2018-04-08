using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.BL.Dto
{
    public class SimilarEntityDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Id { get; set; }
        public string Database { get; set; }
        public string Link { get; set; }
        public bool IsToImport { get; set; }
        public bool IsExternal => !string.IsNullOrEmpty(Link) && !string.IsNullOrEmpty(Database);
    }
}
