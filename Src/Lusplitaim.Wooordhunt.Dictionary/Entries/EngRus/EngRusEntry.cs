
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    public class EngRusEntry : IEngRusEntry
    {
        private readonly string _htmlContent;

        internal EngRusEntry(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public async Task<EngRusEntryContent> GetContent()
        {
            var transcrTask = GetTranscriptions();
            var wordClassesTask = GetWordClasses();
            var collocTask = GetCollocations();
            var sentenceTask = GetSentences();
            var wordFormTask = GetWordForms();
            var similarWordsTask = GetSimilarWords();
            var phrasalVerbsTask = GetPhrasalVerbs();

            var tasks = new List<Task>() { transcrTask, wordClassesTask,
                collocTask, sentenceTask, wordFormTask, similarWordsTask,
                phrasalVerbsTask };

            await Task.WhenAll(tasks);

            var wordContent = new EngRusEntryContent()
            {
                WordClasses = wordClassesTask.Result,
                Transcriptions = transcrTask.Result,
                Collocations = collocTask.Result,
                Sentences = sentenceTask.Result,
                WordForms = wordFormTask.Result,
                SimilarWords = similarWordsTask.Result,
                PhrasalVerbs = phrasalVerbsTask.Result,
            };

            return wordContent;
        }

        public Task<IEnumerable<TranscriptionSet>?> GetTranscriptions()
        {
            var transcriptionParser = new TranscriptionSetParser(_htmlContent);
            return transcriptionParser.ParseTranscriptions();
        }

        public Task<IEnumerable<EngRusWordClass>?> GetWordClasses()
        {
            IWordClassParser<EngRusWordClass> wordClassParser = 
                new EngRusWordClassParser(_htmlContent);
            return wordClassParser.ParseWordClasses();
        }

        public Task<IEnumerable<Phrase>?> GetCollocations()
        {
            ICollocationParser collocationParser = 
                new CollocationParser(_htmlContent);

            return collocationParser.ParseCollocations();
        }

        public Task<IEnumerable<Phrase>?> GetSentences()
        {
            ISentenceParser sentenceParser =
                new SentenceParser(_htmlContent);

            return sentenceParser.ParseSentences();
        }

        public Task<IEnumerable<Phrase>?> GetSimilarWords()
        {
            ISimilarWordParser similarWordParser = 
                new EngRusSimilarWordParser(_htmlContent);

            return similarWordParser.ParseSimilarWords();
        }

        public Task<IEnumerable<Phrase>?> GetPhrasalVerbs()
        {
            IPhrasalVerbParser phrasalVerbParser = 
                new PhrasalVerbParser(_htmlContent);
            return phrasalVerbParser.ParsePhrasalVerbs();
        }

        public Task<IEnumerable<WordForm>?> GetWordForms()
        {
            IWordFormParser wordFormParser = 
                new WordFormParser(_htmlContent);

            return wordFormParser.ParseWordForms();
        }
    }
}
