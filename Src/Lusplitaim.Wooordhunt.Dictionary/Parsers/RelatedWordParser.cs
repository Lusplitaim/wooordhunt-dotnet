using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal class RelatedWordParser : IRelatedWordParser
    {
        private readonly string _htmlContent;

        private const string wordContentId = "wd_content";
        private const string relatedWordsHeaderText = "Родственные слова, либо редко употребляемые в данном значении";

        internal RelatedWordParser(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public async Task<IEnumerable<Equivalent>?> ParseRelatedWords()
        {
            var wordContent = await GetWordContent();

            if (wordContent is null) return default;

            var relatedWordNodes = GetRelatedWordNodes(wordContent);

            if (relatedWordNodes is null) return default;

            return ParseRelatedWordsFrom(relatedWordNodes);
        }

        private async Task<IElement?> GetWordContent()
        {
            var htmlDoc = await new HtmlParser().ParseDocumentAsync(_htmlContent);

            if (htmlDoc is null) return default;

            return htmlDoc.GetElementById(wordContentId);
        }

        private IEnumerable<INode>? GetRelatedWordNodes(IElement wordContent)
        {
            var nodes = wordContent.GetNodes<INode>(deep: false);

            var relatedWordsHeader = nodes
                .FirstOrDefault(e => e.TextContent == relatedWordsHeaderText);

            if (relatedWordsHeader is null) return default;

            return nodes.GetNodesAfter(relatedWordsHeader);
        }

        private IEnumerable<Equivalent> ParseRelatedWordsFrom(IEnumerable<INode> relatedWordNodes)
        {
            var equivalentParser = new RusEngEquivalentParser(_htmlContent);

            return equivalentParser.ParseEquivalentsFrom(relatedWordNodes);
        }
    }
}
