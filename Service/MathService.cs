using Service.Contracts;
using Entities.TransferObjects;
using Service.Utilities;

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



    }
}
