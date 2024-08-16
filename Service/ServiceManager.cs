using Service.Contracts;
using MemoryStorage.Interfaces;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ITextService> _textService;
        private readonly Lazy<IMathService> _mathService;

        public ServiceManager(ILoggerManager logger, IMemoryStorage storage)
        {
            _mathService = new Lazy<IMathService>(() => new MathService(logger, storage));
            _textService = new Lazy<ITextService>(() => new TextService(logger, storage));
           
        }

        public IMathService MathService => _mathService.Value;
        public ITextService TextService => _textService.Value;
    }
}
