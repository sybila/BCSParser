using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.BL.Dto
{
    public class BiochemicalEntityDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string VisualisationXml { get; set; }
        public bool Active { get; set; }
        public int SelectedHierarchyType { get; set; }
        public List<BiochemicalEntityTypeDto> HierarchyTypes { get; set; }
        public BiochemicalEntityLinkDto Parent { get; set; }
        public List<BiochemicalEntityLinkDto> Components { get; set; }
        public List<BiochemicalEntityLinkDto> Locations { get; set; }
        public List<ClassificationDto> Classifications { get; set; }
        public List<EntityNoteDto> Notes { get; set; }
    }
}
