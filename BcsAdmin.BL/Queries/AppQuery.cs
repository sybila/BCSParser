using BcsAdmin.BL.Queries.Base;
using BcsAdmin.DAL.Api;
using DotVVM.Framework.Controls;
using Newtonsoft.Json;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Queries
{
    public abstract class AppApiQuery<TResult> : WebQueryBase<TResult>
    {
        public string AppUrl { get; set; } = "https://api.e-cyanobacterium.org";
        public string RepoName { get; set; }

        protected  async Task<IQueryable<TSourceEntity>> GetWebDataAsync<TSourceEntity>(CancellationToken cancellationToken, string repoName)
        {
            var sw = new Stopwatch();
            sw.Start();
            var list = await ApiHelper.GetWebDataAsync<TSourceEntity>(cancellationToken, AppUrl, repoName);
            sw.Stop();
            Console.Write($"Query for: {typeof(TResult).Name} {sw.ElapsedMilliseconds}ms\n");
            return list.AsQueryable();
        }

    }
}
