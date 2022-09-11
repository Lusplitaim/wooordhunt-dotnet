
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    internal interface IWordFormParser
    {
        public Task<IEnumerable<WordForm>?> ParseWordForms();
    }
}
