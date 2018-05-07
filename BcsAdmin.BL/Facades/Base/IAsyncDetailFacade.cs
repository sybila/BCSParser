using System.Threading.Tasks;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Facades
{
    public interface IAsyncDetailFacade<TDetailDTO, TKey>
       where TDetailDTO : IEntity<TKey>
    {
        Task DeleteAsync(TKey id);
        Task<TDetailDTO> GetDetailAsync(TKey id);
        TDetailDTO InitializeNew();
        Task<TDetailDTO> SaveAsync(TDetailDTO detail);
    }
}