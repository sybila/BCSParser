using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Facades.Exceptions
{
    public class InvalidEntityTypeException : Exception
    {
        public InvalidEntityTypeException(string message)
            :base(message)
        {
        }
    }
}
