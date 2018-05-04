using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsAdmin.DAL
{
    public class ApiDownException : Exception
    {
        public ApiDownException(string message) : base($"E-Cyano API seems have expirienced an issue. {message}")
        {
        }

        public ApiDownException(string message, Exception innerException) : base($"E-Cyano API seems have expirienced an issue. {message}", innerException)
        {
        }
    }
}
