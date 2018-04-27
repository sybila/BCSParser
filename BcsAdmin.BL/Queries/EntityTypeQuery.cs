using BcsAdmin.DAL.Api;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riganti.Utils.Infrastructure.Core;
using System.Threading;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Queries
{
    public class EntityTypeNamesQuery : AppApiQuery<string>
    {
        protected async override Task<IQueryable<string>> GetQueriableAsync(CancellationToken cancellationToken)
        {
            var query = await GetWebDataAsync<ApiEntity>(cancellationToken, "entities");

            //var r = query
            //    .Where(e => e.Type.HasValue)
            //    .Select(e => e.Type.Value)
            //    .Distinct()
            //    .Select(e => e.ToString("F"));

            //var a = r.ToList();

            return new List<string>() {
                "Compartment",
                "Atomic",
                "Structure",
                "Complex"}
            .AsQueryable();


        }
    }
}
