﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Repositories.Api.Exceptions
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message) : base(message)
        {
        }
    }
}
