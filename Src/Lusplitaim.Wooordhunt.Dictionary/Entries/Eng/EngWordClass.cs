
using System.Collections.Generic;

namespace Lusplitaim.Wooordhunt
{
    public class EngWordClass
    {
        public string TypeName { get; }
        public IEnumerable<EngDefinition> Definitions { get; set; }

        public EngWordClass(string typeName)
        {
            TypeName = typeName;
            Definitions = new List<EngDefinition>();
        }
    }
}
