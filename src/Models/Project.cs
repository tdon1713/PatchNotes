using System;
using System.Collections.Generic;

namespace PatchNotes.Models
{
    public class Project
    {
        public Project() { }

        public Guid ID { get; set; }

        public Guid CompanyID { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public bool Archived { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<Version> Versions { get; set; } = new List<Version>();
    }
}
