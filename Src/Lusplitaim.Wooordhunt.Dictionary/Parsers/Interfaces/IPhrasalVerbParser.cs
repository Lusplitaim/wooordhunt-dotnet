
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal interface IPhrasalVerbParser
    {
        public Task<IEnumerable<Phrase>?> ParsePhrasalVerbs();
    }
}
