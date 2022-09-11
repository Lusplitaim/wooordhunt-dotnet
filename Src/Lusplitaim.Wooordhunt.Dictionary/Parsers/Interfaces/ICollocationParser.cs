
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal interface ICollocationParser
    {
        public Task<IEnumerable<Phrase>?> ParseCollocations();
    }
}
