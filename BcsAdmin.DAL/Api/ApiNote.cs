using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsAdmin.DAL.Api
{
    public class ApiNote : IEntity<int>
    {
        public int Id { get; set; }
        public string UserName {get;set;}
        public string Inserted { get; set; }
        public string Updated { get; set; }
        public string Text { get; set; }
    }
}
