using Service.Contracts;

namespace Service
{
    internal sealed class MathService: IMathService
    {
        private readonly ILoggerManager _logger;

        public MathService(ILoggerManager logger)
        {
            _logger = logger;
        }
        public int Sum(int a, int b) => a + b;

        public int Sub(int a, int b) => a - b;
    }
}
