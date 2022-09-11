
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    public interface IEngEntry
    {
        public Task<IEnumerable<EngWordClass>?> GetWordClasses();
        public Task<IEnumerable<TranscriptionSet>?> GetTranscriptions();
        public Task<EngEntryContent> GetContent();
    }
}
