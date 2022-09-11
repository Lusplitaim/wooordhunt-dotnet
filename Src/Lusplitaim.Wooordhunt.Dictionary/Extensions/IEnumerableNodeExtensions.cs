using AngleSharp.Dom;
using System.Collections.Generic;
using System.Linq;

namespace Lusplitaim.Wooordhunt
{
    internal static class IEnumerableNodeExtensions
    {
        internal static IEnumerable<INode> GetNodesAfter(this IEnumerable<INode> nodes, INode node)
        {
            var nodesArray = nodes.ToArray();
            var startingIndex = nodesArray.Index(node);
            return nodesArray[startingIndex..];
        }

        internal static IEnumerable<INode> GetNodesBefore(this IEnumerable<INode> nodes, INode node)
        {
            var nodesArray = nodes.ToArray();
            var endingIndex = nodesArray.Index(node);
            return nodesArray[..endingIndex];
        }
    }
}
