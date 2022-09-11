
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal interface IRelatedWordParser
    {
        public Task<IEnumerable<Equivalent>?> ParseRelatedWords();
    }
}
