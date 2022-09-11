using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    public class RusEngEntry : IRusEngEntry
    {
        private readonly string _htmlContent;

        internal RusEngEntry(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public async Task<RusEngEntryContent> GetContent()
        {
            var equivTask = GetEquivalents();
            var relatedWordsTask = GetRelatedWords();
            var collocTask = GetCollocations();
            var sentenceTask = GetSentences();

            var tasks = new List<Task>() 
                { equivTask, relatedWordsTask, collocTask, sentenceTask };

            await Task.WhenAll(tasks);

            var wordContent = new RusEngEntryContent()
            {
                Equivalents = equivTask.Result,
                RelatedWords = relatedWordsTask.Result,
                Collocations = collocTask.Result,
                Sentences = sentenceTask.Result
            };

            return wordContent;
        }

        public Task<IEnumerable<Equivalent>?> GetEquivalents()
        {
            IEquivalentParser equivalentParser = new RusEngEquivalentParser(_htmlContent);
            return equivalentParser.ParseEquivalents();
        }

        public Task<IEnumerable<Phrase>?> GetCollocations()
        {
            ICollocationParser collocationParser = new RusEngCollocationParser(_htmlContent);
            return collocationParser.ParseCollocations();
        }

        public Task<IEnumerable<Equivalent>?> GetRelatedWords()
        {
            IRelatedWordParser relatedWordParser = new RelatedWordParser(_htmlContent);
            return relatedWordParser.ParseRelatedWords();
        }

        public Task<IEnumerable<Phrase>?> GetSentences()
        {
            ISentenceParser sentenceParser = new SentenceParser(_htmlContent);
            return sentenceParser.ParseSentences();
        }
    }
}
