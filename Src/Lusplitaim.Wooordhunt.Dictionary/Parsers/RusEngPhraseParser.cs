using AngleSharp.Dom;
using System.Collections.Generic;
using System.Linq;

namespace Lusplitaim.Wooordhunt
{
    internal static class RusEngPhraseParser
    {
        private const string engVariantTagName = "SPAN";

        internal static IEnumerable<Phrase> ParsePhrases(IElement phrasesDiv)
        {
            var engVarNodes = phrasesDiv.ChildNodes
                .GetElementsByTagName(engVariantTagName)
                .Where(e => e.ClassName == null);

            var phrases = new List<Phrase>();

            foreach (var engVarNode in engVarNodes)
            {
                string rusVariant = engVarNode.PreviousSibling!.TextContent.RemoveDash();
                string engVariant = engVarNode.TextContent;

                phrases.Add(new Phrase(rusVariant, engVariant));
            }

            return phrases;
        }
    }
}
