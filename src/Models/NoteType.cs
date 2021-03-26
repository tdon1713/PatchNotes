using PatchNotes.Utility;

namespace PatchNotes.Models
{
    public class NoteType
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public Enumerations.NoteType Type => (Enumerations.NoteType)ID;
    }
   
}
