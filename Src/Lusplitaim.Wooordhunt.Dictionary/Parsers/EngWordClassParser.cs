using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal class EngWordClassParser : IWordClassParser<EngWordClass>
    {
        private readonly string _htmlContent;

        private IEngDefinitionParser? _definitionParser;

        private const string contentInEnglishId = "#content_in_english";
        private const string dropdownWordClassNameSelector = ".pos_item.pos_item_link";
        private const string wordClassNameSelector = ".pos_item";

        internal EngWordClassParser(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public async Task<IEnumerable<EngWordClass>?> ParseWordClasses()
        {
            var wordClassNameElements = await GetWorldClassNameElements();

            if (wordClassNameElements is null) return default;

            return ParseWordClassesOf(wordClassNameElements);
        }

        private async Task<IEnumerable<IElement>?> GetWorldClassNameElements()
        {
            var engContentDiv = await GetEnglishContentElement();

            var wordClassNameElements = engContentDiv?
                .QuerySelectorAll($"{wordClassNameSelector}, {dropdownWordClassNameSelector}");

            return wordClassNameElements;
        }

        private async Task<IElement?> GetEnglishContentElement()
        {
            var htmlParser = new HtmlParser();

            var htmlDocument = await htmlParser.ParseDocumentAsync(_htmlContent);
            if (htmlDocument is null) return default;

            return htmlDocument.QuerySelector(contentInEnglishId)!;
        }

        private IEnumerable<EngWordClass> ParseWordClassesOf(IEnumerable<IElement> wordClassNameElements)
        {
            var wordClasses = new List<EngWordClass>();
            foreach (var wordClassNameElement in wordClassNameElements)
            {
                var wordClass = ParseWordClassOf(wordClassNameElement);

                if (wordClass != null)
                    wordClasses.Add(wordClass);
            }
            return wordClasses;
        }

        private EngWordClass? ParseWordClassOf(IElement wordClassNameElement)
        {
            var wordClassName = wordClassNameElement.TextContent.RemoveArrow();

            var definitions = ParseDefinitionsOf(wordClassNameElement);

            if (definitions is null) return default;

            return new EngWordClass(wordClassName) { Definitions = definitions };
        }

        private IEnumerable<EngDefinition>? ParseDefinitionsOf(IElement wordClassNameElement)
        {
            var wordDefinitionsDiv = wordClassNameElement.NextElementSibling;

            if (wordDefinitionsDiv is null) return default;

            _definitionParser = new EngDefinitionParser(wordDefinitionsDiv);

            return _definitionParser.ParseDefinitions();
        }
    }
}
