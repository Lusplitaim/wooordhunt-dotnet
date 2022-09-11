
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal interface ISentenceParser
    {
        public Task<IEnumerable<Phrase>?> ParseSentences();
    }
}
