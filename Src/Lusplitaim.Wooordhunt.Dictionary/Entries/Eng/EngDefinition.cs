
using System.Collections.Generic;

namespace Lusplitaim.Wooordhunt
{
    public class EngDefinition
    {
        public string Definition { get; set; }
        public IEnumerable<string>? Examples { get; set; }

        public EngDefinition(string definition)
        {
            Definition = definition;
        }
    }
}
