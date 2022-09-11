using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal class RusEngCollocationParser : ICollocationParser
    {
        private readonly string _htmlContent;

        private const string collocDivSelector = "div.word_ex.word_ex_sup";

        internal RusEngCollocationParser(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public async Task<IEnumerable<Phrase>?> ParseCollocations()
        {
            var htmlDoc = await new HtmlParser().ParseDocumentAsync(_htmlContent);

            return await ParseCollocationsFrom(htmlDoc);
        }

        private async Task<IEnumerable<Phrase>?> ParseCollocationsFrom(IHtmlDocument htmlDoc)
        {
            var collocDiv = htmlDoc.QuerySelector(collocDivSelector);

            if (collocDiv is null)
            {
                ICollocationParser collocParser = new CollocationParser(_htmlContent);
                return await collocParser.ParseCollocations();
            }

            return new List<Phrase>(ParseCollocationsFrom(collocDiv));
        }

        private IEnumerable<Phrase> ParseCollocationsFrom(IElement collocationsDiv)
        {
            return RusEngPhraseParser.ParsePhrases(collocationsDiv);
        }
    }
}
