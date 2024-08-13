using Service.Contracts;
using Entities.TransferObjects;

namespace Service
{
    internal sealed class MathService: IMathService
    {
        private readonly ILoggerManager _logger;

        public MathService(ILoggerManager logger)
        {
            _logger = logger;
        }
        public int GetSum(int a, int b) => a + b;

        public int GetSub(int a, int b) => a - b;

        public int GetSum(NumberInputModel numbers) => numbers.Numbers.Sum();

        public double GetAverage(NumberInputModel numbers) => numbers.Numbers.Average();

    }
}
