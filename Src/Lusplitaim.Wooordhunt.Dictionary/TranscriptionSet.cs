
namespace Lusplitaim.Wooordhunt
{
    public class TranscriptionSet
    {
        public string? Note { get; set; }
        public string? British { get; set; }
        public string? American { get; set; }

        public TranscriptionSet(string? note, string? british, string? american)
        {
            Note = note;
            British = british;
            American = american;
        }
    }
}
