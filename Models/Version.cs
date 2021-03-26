using System;

namespace PatchNotes.Models
{
    public class Version
    {
        public Guid ID { get; set; }

        public Guid ProjectID { get; set; }

        public int Major { get; set; }

        public int Minor { get; set; }

        public int Revision { get; set; }

        public int Build { get; set; }

        public string Postfix { get; set; }

        public string Notes { get; set; }

        public bool Archived { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ReleasedDate { get; set; }
    }
}
