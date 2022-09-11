using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal class CollocationParser : ICollocationParser
    {
        private readonly string _htmlContent;

        private const string collocDivClassName = "block phrases";
        private const string hiddenCollocDivClassName = "hidden";
        private const string collocBlockRusHeader = "Словосочетания";
        private const string textNodeName = "#text";
        private const string boldTextNodeName = "B";
        private const string linkedTextNodeName = "A";
        private const string italicTextNodeName = "I";

        internal CollocationParser(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public async Task<IEnumerable<Phrase>?> ParseCollocations()
        {
            var collocDiv = await GetCollocationsDiv();

            if (collocDiv is null) return default;

            return ParseCollocationsDiv(collocDiv);
        }

        private async Task<IElement?> GetCollocationsDiv()
        {
            var collocDiv = await GetCollocationsDivViaHeader();

            if (collocDiv?.ClassName != collocDivClassName)
                return default;

            return collocDiv;
        }

        private async Task<IElement?> GetCollocationsDivViaHeader()
        {
            var htmlDocument = await new HtmlParser().ParseDocumentAsync(_htmlContent);

            var collocDivHeader = htmlDocument.All
                .FirstOrDefault(e => e.TextContent == collocBlockRusHeader);

            var collocDiv = collocDivHeader?.NextElementSibling;

            return collocDiv;
        }

        private List<Phrase> ParseCollocationsDiv(IElement collocDiv)
        {
            var collocDivNodes = collocDiv.GetNodes<INode>(deep: false);

            var collocations = new List<Phrase>();
            (string engVar, string rusVar) = ("", "");
            foreach (var node in collocDivNodes)
            {
                bool isEngVariantPart = node.NodeName == textNodeName
                    || node.NodeName == linkedTextNodeName
                    || node.NodeName == boldTextNodeName;

                if (isEngVariantPart)
                {
                    engVar += node.TextContent;
                    continue;
                }

                bool isRusVariant = node.NodeName == italicTextNodeName;

                if (isRusVariant)
                {
                    rusVar = node.TextContent;
                    engVar = engVar.RemoveDash();
                    collocations.Add(new Phrase(engVar, rusVar));
                    (engVar, rusVar) = ("", "");
                }

                var element = node as IElement;
                bool isHiddenCollocDiv = element?.ClassName == hiddenCollocDivClassName;

                if (isHiddenCollocDiv)
                {
                    collocations.AddRange(ParseCollocationsDiv(element!));
                }
            }

            return collocations;
        }
    }
}
