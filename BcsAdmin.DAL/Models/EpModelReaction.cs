﻿using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpModelReaction
    {
        public int Id { get; set; }
        public int? ModelId { get; set; }
        public int? FunctionId { get; set; }
        public string Name { get; set; }
    }
}
