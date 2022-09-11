using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal class RusEngEquivalentParser : IEquivalentParser
    {
        private readonly string _htmlContent;

        private const string contentId = "wd_content";
        private const string equivalentNodeName = "A";
        private const string examplesClassName = "word_ex";
        private const string gapDivClassName = "gap";

        internal RusEngEquivalentParser(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public async Task<IEnumerable<Equivalent>?> ParseEquivalents()
        {
            var equivalentNodes = await GetEquivalentNodes();

            if (equivalentNodes is null) return default;

            return ParseEquivalentsFrom(equivalentNodes);
        }

        private async Task<IEnumerable<INode>?> GetEquivalentNodes()
        {
            var htmlDoc = await new HtmlParser().ParseDocumentAsync(_htmlContent);

            var htmlEntryContent = htmlDoc.GetElementById(contentId);

            if (htmlEntryContent is null) return default;

            var closingElement = htmlEntryContent
                .Children.FirstOrDefault(e => e.ClassName == gapDivClassName);

            if (closingElement is null) return default;

            return htmlEntryContent.ChildNodes.GetNodesBefore(closingElement);
        }

        internal IEnumerable<Equivalent> ParseEquivalentsFrom(IEnumerable<INode> equivalentNodes)
        {
            var equivalents = new List<Equivalent>();

            var nodesWithEquivalentNames = equivalentNodes
                .Where(n => n.NodeName == equivalentNodeName);

            foreach (var nodeWithEquivalentName in nodesWithEquivalentNames)
            {
                var equivalent = ParseEquivalentStartingWith(nodeWithEquivalentName);
                equivalents.Add(equivalent);
            }

            return equivalents;
        }

        private Equivalent ParseEquivalentStartingWith(INode startingNode)
        {
            var equivalentName = startingNode.TextContent;

            INode transcriptionNode = startingNode.NextSibling!;

            var transcription = transcriptionNode.TextContent;

            INode translationNode = transcriptionNode.NextSibling!;

            var translation = translationNode.TextContent;

            var examplesDiv = TryGettingExamplesDiv((translationNode as IElement)!);

            IEnumerable<Phrase>? examples = default;

            if (examplesDiv != null)
            {
                examples = ParseEquivalentExamples(examplesDiv);
            }

            var equivalent = new Equivalent
            {
                EnglishWord = equivalentName,
                Transcription = transcription,
                Translation = translation,
                Examples = examples
            };

            return equivalent;
        }

        private IElement? TryGettingExamplesDiv(IElement translationElement)
        {
            var examplesDiv = translationElement.NextElementSibling?.NextElementSibling;

            if (examplesDiv?.ClassName == examplesClassName)
            {
                return examplesDiv;
            }

            return default;
        }

        private IEnumerable<Phrase> ParseEquivalentExamples(IElement examplesDiv)
        {
            return RusEngPhraseParser.ParsePhrases(examplesDiv);
        }
    }
}
