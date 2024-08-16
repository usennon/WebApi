using Service.Contracts;
using Entities.TransferObjects;
using Service.Utilities;
using MemoryStorage.Interfaces;

namespace Service
{
    internal sealed class MathService : IMathService
    {
        private readonly ILoggerManager _logger;

        private readonly IMemoryStorage _storage;

        public MathService(ILoggerManager logger, IMemoryStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }
        public int GetSum(int a, int b)
        {
            int sum = _storage.GetCurrentTotal() + a + b;
            _storage.AddResult(sum);
            return sum;
        }

        public int GetSub(int a, int b)
        {
            int sum = _storage.GetCurrentTotal() - a - b;
            _storage.AddResult(sum);
            return sum;
        }

        public int GetSum(NumberInputModel numbers)
        {
            int sum = _storage.GetCurrentTotal() + numbers.Numbers.Sum();
            _storage.AddResult(sum);
            return sum;
        }

        public double GetAverage(NumberInputModel numbers) => numbers.Numbers.Average();

        public double GetIntegral(IntegralParametersModel parameters)
            => SimpsonMethod.SimpsonIntegral(parameters.StartInterval, parameters.EndInterval, parameters.IntervalsAmount);

        public double GetCompoundInterest(CompoundInterstModel model)
        {
            if (model.ifReinvestment == true)
            {
                return model.StartSum * Math.Pow(1 + model.YearInterestRate / model.NumberOfPeriods,
                    model.NumberOfPeriods * model.YearsNumber);
            }
            else
            {
                return model.StartSum * (1 + model.YearInterestRate * model.YearsNumber);
            }
        }

        public void Clear() => _storage.Clear();

    }
}
