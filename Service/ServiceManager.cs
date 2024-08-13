using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ITextService> _textService;
        private readonly Lazy<IMathService> _mathService;

        public ServiceManager(ILoggerManager logger)
        {
            _mathService = new Lazy<IMathService>(() => new MathService(logger));
            _textService = new Lazy<ITextService>(() => new TextService(logger));
           
        }

        public IMathService MathService => _mathService.Value;
        public ITextService TextService => _textService.Value;
    }
}
