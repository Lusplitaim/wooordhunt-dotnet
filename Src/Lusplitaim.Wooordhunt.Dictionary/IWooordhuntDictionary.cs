
using System;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    public interface IWooordhuntDictionary : IAsyncDisposable
    {
        public Task<IEngRusEntry> GetEngRusEntry(string englishWord);
        public Task<IEngEntry> GetEngEntry(string englishWord);
        public Task<IRusEngEntry> GetRusEngEntry(string russianWord);
    }
}
