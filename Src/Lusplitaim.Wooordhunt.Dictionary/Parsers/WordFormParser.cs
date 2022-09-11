using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal class WordFormParser : IWordFormParser
    {
        private readonly string _htmlContent;

        private const string wordFormDivSelector = ".word_form_block";
        private const string formTagName = "SPAN";

        internal WordFormParser(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public async Task<IEnumerable<WordForm>?> ParseWordForms()
        {
            var wordFormDivs = await GetWordFormDivs();

            if (wordFormDivs is null) return default;

            return ParseWordFormsFrom(wordFormDivs);
        }

        private async Task<IEnumerable<IElement>> GetWordFormDivs()
        {
            var htmlDocument = await new HtmlParser().ParseDocumentAsync(_htmlContent);

            return htmlDocument.QuerySelectorAll(wordFormDivSelector);
        }

        private IEnumerable<WordForm> ParseWordFormsFrom(IEnumerable<IElement> wordFormDivs)
        {
            var wordForms = new List<WordForm>();
            foreach (var wordFormDiv in wordFormDivs)
            {
                wordForms.AddRange(ParseWordFormsDiv(wordFormDiv));
            }
            return wordForms;
        }

        private IEnumerable<WordForm> ParseWordFormsDiv(IElement wordFormDiv)
        {
            var formElements = wordFormDiv.Children.Where(e => e.TagName == formTagName);
            var wordForms = new List<WordForm>();

            foreach (var formElement in formElements)
            {
                var wordElement = GetWordNodeUsing(formElement);

                var form = formElement.TextContent.Trim();
                var word = wordElement!.TextContent.Trim();

                wordForms.Add(new WordForm(form, word));
            }

            return wordForms;
        }

        private INode? GetWordNodeUsing(IElement formElement)
        {
            var wordElement = formElement.NextSibling;

            if (wordElement?.TextContent == " ")
                wordElement = formElement.NextElementSibling;

            return wordElement;
        }
    }
}
