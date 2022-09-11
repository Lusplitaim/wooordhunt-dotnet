
namespace Lusplitaim.Wooordhunt
{
    public class WordForm
    {
        public string Form { get; set; }
        public string Word { get; set; }

        public WordForm(string form, string word)
        {
            Form = form;
            Word = word;
        }
    }
}
