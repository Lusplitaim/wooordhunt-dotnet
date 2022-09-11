using AngleSharp.Dom;
using System.Collections.Generic;
using System.Linq;

namespace Lusplitaim.Wooordhunt
{
    internal static class SimilarWordParser
    {
        private const string originalVariantTagName = "A";

        internal static IEnumerable<Phrase>? ParseSimilarWordsDiv(IElement similarWordsDiv)
        {
            var originalVariants = 
                similarWordsDiv.Children.Where(e => e.TagName == originalVariantTagName);

            if (originalVariants is null) return default;

            return ParseSimilarWordsHaving(originalVariants);
        }

        private static IEnumerable<Phrase> ParseSimilarWordsHaving(IEnumerable<IElement> origVariants)
        {
            var similarWords = new List<Phrase>();

            foreach (var origVariant in origVariants)
            {
                var translatedVariant = origVariant.NextSibling;

                if (translatedVariant is null) continue;

                var original = origVariant.TextContent.Trim();
                var translation = translatedVariant.TextContent.RemoveDash();

                similarWords.Add(new Phrase(original, translation));
            }

            return similarWords;
        }
    }
}
