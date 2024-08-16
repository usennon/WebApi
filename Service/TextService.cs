using Service.Contracts;
using MemoryStorage.Interfaces;
using System.Text;

namespace Service
{
    internal sealed class TextService : ITextService
    {
        private readonly ILoggerManager _logger;

        private readonly IMemoryStorage _storage;

        public TextService(ILoggerManager logger, IMemoryStorage storage) 
        {
            _logger = logger;
            _storage = storage;
        }
        public string ToUpperCase(string str) => str.ToUpper();

        public string Concatenate(string str1, string str2)
        {
            return _storage.Concatenate(str1, str2);
        }

        public void Clear() => _storage.Clear(); 
    }
}
