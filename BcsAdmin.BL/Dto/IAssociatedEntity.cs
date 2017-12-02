using System;
using System.Collections.Generic;
using System.Text;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Dto
{
    public interface IAssociatedEntity : IEntity<int>
    {
        int? DetailEntityId { get; set; }
    }
}
