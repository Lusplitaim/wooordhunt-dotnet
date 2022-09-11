using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal class PhrasalVerbParser : IPhrasalVerbParser
    {
        private readonly string _htmlContent;

        private const string phrasalVerbsDivHeader = "Фразовые глаголы";
        private const string phrasalVerbsHeaderTag = "h3";

        internal PhrasalVerbParser(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public async Task<IEnumerable<Phrase>?> ParsePhrasalVerbs()
        {
            var htmlParser = new HtmlParser();

            var htmlDocument = await htmlParser.ParseDocumentAsync(_htmlContent);

            var phrasalVerbsHeader = htmlDocument.QuerySelectorAll(phrasalVerbsHeaderTag)
                .FirstOrDefault(e => e.TextContent == phrasalVerbsDivHeader);

            if (phrasalVerbsHeader is null) return default;

            var phrasalVerbsDiv = phrasalVerbsHeader.NextElementSibling;

            if (phrasalVerbsDiv is null) return default;

            return SimilarWordParser.ParseSimilarWordsDiv(phrasalVerbsDiv);
        }
    }
}
