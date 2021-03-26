using System;
using System.Collections.Generic;

namespace PatchNotes.Models
{
    public class Company
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public bool Archived { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedDate_Display => CreatedDate.ToString("M/dd/yyyy H:mm tt");

        public List<Project> Projects { get; set; } = new List<Project>();

        public List<Version> Versions { get; set; } = new List<Version>();

        public override string ToString()
        {
            return Name;
        }

        public bool IsNew => ID == Guid.Empty;
    }
}
