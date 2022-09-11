
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal interface ISimilarWordParser
    {
        public Task<IEnumerable<Phrase>?> ParseSimilarWords();
    }
}
