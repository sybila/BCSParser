using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BcsAdmin.BL
{
    public class ApiUnitOfWorkProvider : IUnitOfWorkProvider
    {
        public IUnitOfWork Create()
        {
            return new ApiUnitOfWork();
        }

        public IUnitOfWork GetCurrent(int ancestorLevel = 0)
        {
            return new ApiUnitOfWork();
        }
    }

    public class ApiUnitOfWork : IUnitOfWork
    {
        public event EventHandler Disposing;

        public void Commit()
        {
            CommitAsync().Wait();
        }

        public Task CommitAsync()
        {
            return CommitAsync(CancellationToken.None);
        }

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

        public void RegisterAfterCommitAction(Action action)
        {
        }
    }
}
