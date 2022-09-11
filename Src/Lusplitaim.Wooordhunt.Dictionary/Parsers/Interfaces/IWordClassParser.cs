using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal interface IWordClassParser<TWordClass>
    {
        public Task<IEnumerable<TWordClass>?> ParseWordClasses();
    }
}
