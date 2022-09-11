using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal class TranscriptionSetParser
    {
        private readonly string _htmlContent;

        private TranscriptionSet? _transcriptionSet;
        private List<TranscriptionSet>? _transcriptionSets;

        private const string transcriptionBlockSelector = ".trans_sound > div";
        private const string transcriptionNoteSelector = ".es_i";
        private const string ukTransSelector = "#uk_tr_sound .transcription";
        private const string usTransSelector = "#us_tr_sound .transcription";

        internal TranscriptionSetParser(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public async Task<IEnumerable<TranscriptionSet>?> ParseTranscriptions()
        {
            var transcriptionSetDivs = await GetTranscriptionSetDivs();

            ParseTranscriptionSetDivs(transcriptionSetDivs);

            return _transcriptionSets;
        }

        private void ParseTranscriptionSetDivs(IEnumerable<IElement> transcriptionSetDivs)
        {
            _transcriptionSets = new List<TranscriptionSet>();
            _transcriptionSet = default;

            foreach (var transcriptionSetDiv in transcriptionSetDivs)
            {
                TryGettingTranscriptionNote(transcriptionSetDiv);

                TryGettingAmericanTranscription(transcriptionSetDiv);

                TryGettingBritishTranscription(transcriptionSetDiv);
            }

            if (_transcriptionSet != null)
            {
                _transcriptionSets.Add(_transcriptionSet);
            }
        }

        private async Task<IEnumerable<IElement>> GetTranscriptionSetDivs()
        {
            var htmlParser = new HtmlParser();

            var htmlDocument = await htmlParser.ParseDocumentAsync(_htmlContent);

            return htmlDocument.QuerySelectorAll(transcriptionBlockSelector);
        }

        private void TryGettingTranscriptionNote(IElement transcriptionSetDiv)
        {
            var transcriptionNote = transcriptionSetDiv
                .QuerySelector(transcriptionNoteSelector);

            if (transcriptionNote is null)
            {
                return;
            }

            if (_transcriptionSet != null)
            {
                _transcriptionSets!.Add(_transcriptionSet);
            }
            _transcriptionSet = new TranscriptionSet(transcriptionNote.Text(), default, default);
        }

        private string? TryGettingAmericanTranscription(IElement transcriptionSetDiv)
        {
            var usTranscription = transcriptionSetDiv
                .QuerySelector($"{usTransSelector}");

            return usTranscription?.TextContent;
        }

        private string? TryGettingBritishTranscription(IElement transcriptionSetDiv)
        {
            var ukTranscription = transcriptionSetDiv
                                .QuerySelector($"{ukTransSelector}");

            return ukTranscription?.TextContent;
        }
    }
}
