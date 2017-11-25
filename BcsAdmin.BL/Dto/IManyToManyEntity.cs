using System;
using System.Collections.Generic;
using System.Text;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Dto
{
    public interface IManyToManyEntity : IEntity<int>
    {
        int? IntermediateEntityId { get; set; }
    }
}
