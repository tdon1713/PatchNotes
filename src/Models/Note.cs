using System;

namespace PatchNotes.Models
{
    public class Note
    {
        public Guid ID { get; set; }

        public Guid VersionID { get; set; }

        public int NoteTypeID { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Description { get; set; }

        public string IssueURL { get; set; }

        public string IssueID { get; set; }

        public string DeveloperNote { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
