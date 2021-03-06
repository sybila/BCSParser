﻿using System;
using System.Collections.Generic;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.DAL.Models
{
    public partial class EpReaction : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Equation { get; set; }
        public string Modifier { get; set; }
        public string VisualisationXml { get; set; }
        public int? Active { get; set; }
        public int? IsValid { get; set; }

        ICollection<EpReactionClassification> Classifications { get; set; }
        ICollection<EpReactionNote> Notes { get; set; }
        ICollection<EpReactionOrganism> Organisms { get; set; }
    }
}
