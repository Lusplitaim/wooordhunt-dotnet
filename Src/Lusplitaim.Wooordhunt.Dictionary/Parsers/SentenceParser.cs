using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal class SentenceParser : ISentenceParser
    {
        private readonly string _htmlContent;

        private const string sentencesDivRusHeader = "Примеры";
        private const string sentenceEngVarClassName = "ex_o";
        private const string sentenceRusVarClassName = "ex_t human";

        internal SentenceParser(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public async Task<IEnumerable<Phrase>?> ParseSentences()
        {
            var sentencesDiv = await GetSentencesDiv();

            if (sentencesDiv is null) return default;

            List<Phrase> sentences = ParseSentencesDiv(sentencesDiv);

            return sentences;
        }

        private async Task<IElement?> GetSentencesDiv()
        {
            var htmlDocument = await new HtmlParser().ParseDocumentAsync(_htmlContent);

            var sentencesDivHeader = htmlDocument.All
                .FirstOrDefault(e => e.TextContent == sentencesDivRusHeader);

            if (sentencesDivHeader is null) return default;

            var sentencesDiv = sentencesDivHeader.NextElementSibling;

            return sentencesDiv;
        }

        private List<Phrase> ParseSentencesDiv(IElement sentenceBlock)
        {
            var sentenceOrigPhrases = sentenceBlock.GetElementsByClassName(sentenceEngVarClassName);

            return ParseSentenceElements(sentenceOrigPhrases);
        }

        private List<Phrase> ParseSentenceElements(IEnumerable<IElement> sentenceElements)
        {
            var sentences = new List<Phrase>();

            foreach (var origPhraseElement in sentenceElements)
            {
                var translatedPhraseElement = origPhraseElement.NextElementSibling;

                if (translatedPhraseElement?.ClassName != sentenceRusVarClassName)
                    continue;

                var original = origPhraseElement.TextContent;
                var translation = translatedPhraseElement.TextContent;

                sentences.Add(new Phrase(original, translation));
            }

            return sentences;
        }
    }
}
