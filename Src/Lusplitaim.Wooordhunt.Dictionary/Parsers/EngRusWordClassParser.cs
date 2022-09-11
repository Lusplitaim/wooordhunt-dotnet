using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal class EngRusWordClassParser : IWordClassParser<EngRusWordClass>
    {
        private readonly string _htmlContent;

        private const string wordClassNameSelector = ".pos_item.pos_item_link";

        internal EngRusWordClassParser(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public async Task<IEnumerable<EngRusWordClass>?> ParseWordClasses()
        {
            var wordClassNameElements = await GetWordClassNameElements();

            if (wordClassNameElements is null) return default;

            return ParseWordClassesHaving(wordClassNameElements);
        }

        private async Task<IEnumerable<IElement>?> GetWordClassNameElements()
        {
            var htmlDocument = await new HtmlParser().ParseDocumentAsync(_htmlContent);
            if (htmlDocument is null) return default;

            var wordClassNameElements = htmlDocument
                .QuerySelectorAll(wordClassNameSelector);

            return wordClassNameElements;
        }

        private IEnumerable<EngRusWordClass> ParseWordClassesHaving(IEnumerable<IElement> wordClassNameElements)
        {
            var wordClasses = new List<EngRusWordClass>();

            foreach (var wordClassNameElement in wordClassNameElements)
            {
                wordClasses.Add(ParseWordClassHaving(wordClassNameElement));
            }

            return wordClasses;
        }

        private EngRusWordClass ParseWordClassHaving(IElement wordClassNameElement)
        {
            var wordClassDiv = wordClassNameElement.NextElementSibling;

            var wordClassName = wordClassNameElement.TextContent.RemoveArrow();

            var translations = EngRusTranslationParser.ParseTranslations(wordClassDiv!);

            return new EngRusWordClass(wordClassName) { Translations = translations };
        }
    }
}
