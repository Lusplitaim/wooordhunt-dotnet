using AngleSharp.Dom;
using System.Collections.Generic;

namespace Lusplitaim.Wooordhunt
{
    internal class EngDefinitionParser : IEngDefinitionParser
    {
        private readonly IElement _wordClassDiv;

        private const string hiddenExamplesDivClassName = "ex hidden";
        private const string examplesDivClassName = "ex";
        private const string hiddenDefinitsDivClassName = "hidden";

        private const string definitWithExamplesNodeName = "SPAN";

        private const string textNodeName = "#text";
        private const string brNodeName = "BR";
        private const string definitItalicNodeName = "I";

        internal EngDefinitionParser(IElement wordClassDiv)
        {
            _wordClassDiv = wordClassDiv;
        }

        public IEnumerable<EngDefinition> ParseDefinitions()
        {
            return ParseDefinitions(_wordClassDiv);
        }

        private IEnumerable<EngDefinition> ParseDefinitions(IElement wordClassDiv)
        {
            var wordClassNodes = wordClassDiv.GetNodes<INode>(deep: false);

            var definitions = new List<EngDefinition>();
            string? definitionName = default;
            IEnumerable<string>? definitionExamples = default;
            foreach (var node in wordClassNodes)
            {
                var nodeName = node.NodeName;

                bool isPartOfDefinition = nodeName == textNodeName
                    || nodeName == definitWithExamplesNodeName
                    || nodeName == definitItalicNodeName;
                if (isPartOfDefinition)
                {
                    definitionName += node.TextContent;
                    continue;
                }

                if (nodeName == brNodeName)
                {
                    var nextNodeElement = node.NextSibling as IElement;

                    bool isNextNodeExamplesDiv = nextNodeElement?.ClassName == hiddenExamplesDivClassName 
                        || (nextNodeElement?.ClassName?.Trim() == examplesDivClassName);
                    if (isNextNodeExamplesDiv)
                    {
                        definitionExamples = ParseDefinitionExamples(nextNodeElement!);
                    }

                    definitionName = definitionName!.RemoveDash();
                    definitions.Add(new EngDefinition(definitionName) 
                    { 
                        Examples = definitionExamples
                    });

                    definitionName = default;
                    definitionExamples = default;
                }

                var hiddenDefinitsDiv = node as IElement;
                if (hiddenDefinitsDiv?.ClassName == hiddenDefinitsDivClassName)
                {
                    definitions.AddRange(ParseDefinitions(hiddenDefinitsDiv!));
                }
            }

            return definitions;
        }

        private static IEnumerable<string> ParseDefinitionExamples(IElement examplesDiv)
        {
            IEnumerable<INode> exampleNodes = examplesDiv
                .GetNodes<INode>(deep: false, (node) =>
            {
                if (node.NodeName == textNodeName) return true;
                return false;
            });

            var examples = new List<string>();
            foreach (var exampleNode in exampleNodes)
            {
                var nodeTextContent = exampleNode.TextContent;

                if (exampleNode.NodeName == textNodeName)
                {
                    examples.Add(nodeTextContent.RemoveDash());
                }
            }

            return examples;
        }
    }
}
