using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal interface IEquivalentParser
    {
        public Task<IEnumerable<Equivalent>?> ParseEquivalents();
    }
}
