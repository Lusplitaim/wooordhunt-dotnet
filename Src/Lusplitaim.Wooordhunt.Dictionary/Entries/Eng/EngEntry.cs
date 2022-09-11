
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    public class EngEntry : IEngEntry
    {
        private TranscriptionSetParser _transcriptionParser;
        private IWordClassParser<EngWordClass> _wordClassParser;

        internal EngEntry(string htmlContent)
        {
            _transcriptionParser = new TranscriptionSetParser(htmlContent);
            _wordClassParser = new EngWordClassParser(htmlContent);
        }

        public async Task<EngEntryContent> GetContent()
        {
            var transcriptionTask = GetTranscriptions();
            var wordClassTask = GetWordClasses();

            await Task.WhenAll(transcriptionTask, wordClassTask);

            return new EngEntryContent
            {
                Transcriptions = transcriptionTask.Result,
                WordClasses = wordClassTask.Result,
            };
        }

        public Task<IEnumerable<TranscriptionSet>?> GetTranscriptions()
        {
            return _transcriptionParser.ParseTranscriptions();
        }

        public Task<IEnumerable<EngWordClass>?> GetWordClasses()
        {
            return _wordClassParser.ParseWordClasses();
        }
    }
}
