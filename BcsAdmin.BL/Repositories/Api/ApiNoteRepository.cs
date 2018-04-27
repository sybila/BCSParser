using BcsAdmin.BL.Repositories.Api.BcsAdmin.BL.Repositories;
using BcsAdmin.DAL.Api;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Repositories.Api
{
    public class ApiNoteRepository : ApiGenericRepository<ApiNote>
    {
        public ApiNoteRepository(IDateTimeProvider dateTimeProvider)
            : base(dateTimeProvider)
        {
        }
    }
}
