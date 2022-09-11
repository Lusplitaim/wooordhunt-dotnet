using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal class EngRusSimilarWordParser : ISimilarWordParser
    {
        private readonly string _htmlContent;

        private const string similarWordsDivHeader = "Возможные однокоренные слова";
        private const string similarWordsHeaderTag = "h3";

        internal EngRusSimilarWordParser(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public async Task<IEnumerable<Phrase>?> ParseSimilarWords()
        {
            var htmlParser = new HtmlParser();

            var htmlDocument = await htmlParser.ParseDocumentAsync(_htmlContent);

            var similarWordsHeader = htmlDocument.QuerySelectorAll(similarWordsHeaderTag)
                .FirstOrDefault(e => e.TextContent == similarWordsDivHeader);

            if (similarWordsHeader is null) return default;

            var similarWordsDiv = similarWordsHeader.NextElementSibling;

            if (similarWordsDiv is null) return default;

            return SimilarWordParser.ParseSimilarWordsDiv(similarWordsDiv);
        }
    }
}
