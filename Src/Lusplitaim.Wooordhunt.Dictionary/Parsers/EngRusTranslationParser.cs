using AngleSharp.Dom;
using System.Collections.Generic;
using System.Linq;

namespace Lusplitaim.Wooordhunt
{
    internal class EngRusTranslationParser
    {
        private const string translWithExamplesTagName = "SPAN";
        private const string rusVariantTagName = "I";
        private const string brNodeName = "BR";
        private const string additPartOfTranslTagName = "I";
        private const string textNodeName = "#text";

        public static IEnumerable<EngRusTranslation> ParseTranslations(IElement wordClassDiv)
        {
            var equivalents = new List<EngRusTranslation>();

            equivalents.AddRange(ParseTranslationsFrom(wordClassDiv));

            return equivalents;
        }

        private static IEnumerable<EngRusTranslation> ParseTranslationsFrom(IElement wordClassDiv)
        {
            var nodesWithDashChar = wordClassDiv.GetNodes<INode>()
                .Where(n => n.TextContent.StartsWith('-') && n.NodeName == textNodeName);

            var translations = new List<EngRusTranslation>();

            foreach (var nodeWithDashChar in nodesWithDashChar)
            {
                var translation = ParseTranslationStartingWith(nodeWithDashChar);
                translations.Add(translation);
            }

            return translations;
        }

        private static EngRusTranslation ParseTranslationStartingWith(INode startingNode)
        {
            var nextNode = startingNode.NextSibling;

            bool isTranslationWithExamples =
                (nextNode as Element)?.TagName == translWithExamplesTagName;

            if (isTranslationWithExamples)
            {
                return ParseTranslationWithExamplesFrom((nextNode as Element)!);
            }

            var translation = startingNode.TextContent;

            while (nextNode != null)
            {
                bool isPartOfTranslation =
                    nextNode?.NodeName == additPartOfTranslTagName
                    || nextNode?.NodeName == textNodeName;

                if (isPartOfTranslation)
                {
                    translation += nextNode!.TextContent;
                }

                bool isEndOfTranslation = nextNode?.NodeName == brNodeName;
                if (isEndOfTranslation)
                {
                    break;
                }

                nextNode = nextNode?.NextSibling;
            }

            return new EngRusTranslation(translation.RemoveDash());
        }

        private static EngRusTranslation ParseTranslationWithExamplesFrom(IElement translationElement)
        {
            var examplesDiv = translationElement.NextElementSibling?.NextElementSibling;
            var translation = translationElement.TextContent;

            return new EngRusTranslation(translation) 
            { 
                Examples = ParseTranslationExamples(examplesDiv!) 
            };
        }

        private static IEnumerable<Phrase> ParseTranslationExamples(IElement examplesDiv)
        {
            var translationExamples = new List<Phrase>();

            var rusVarElements = examplesDiv.GetElementsByTagName(rusVariantTagName);

            foreach (var rusVarElement in rusVarElements)
            {
                var rusVariant = rusVarElement.TextContent;
                var engVariant = rusVarElement!.PreviousSibling!.TextContent.RemoveDash();

                translationExamples.Add(new Phrase(engVariant, rusVariant));
            }

            return translationExamples;
        }
    }
}
