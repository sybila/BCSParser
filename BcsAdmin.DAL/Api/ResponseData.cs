using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.DAL.Api
{
    public class ResponseData
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }              
        public int Code { get; set; }
    }


    public class EntityResponseData<TEntity> : ResponseData
    {
        public TEntity Data { get; set; }
    }
   
    public enum ResponseStatus
    {
        Ok,
        Error
    }
}
