using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.SemanticModel.Exceptions
{
    public class UnsupportedEntityException : Exception
    {
        public UnsupportedEntityException(string message = "Unsupported entity type") : base(message)
        {
        }
    }
}
