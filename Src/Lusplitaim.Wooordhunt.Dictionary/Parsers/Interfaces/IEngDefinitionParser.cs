
using System.Collections.Generic;

namespace Lusplitaim.Wooordhunt
{
    internal interface IEngDefinitionParser
    {
        public IEnumerable<EngDefinition> ParseDefinitions();
    }
}
