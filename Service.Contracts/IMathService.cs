using Entities.TransferObjects;

namespace Service.Contracts
{
    public interface IMathService
    {
        public int GetSum(int a, int b);

        public int GetSub(int a, int b);

        public int GetSum(NumberInputModel numbers);

        public double GetAverage(NumberInputModel numbers);

        public double GetIntegral(IntegralParametersModel parameters);

        public double GetCompoundInterest(CompoundInterstModel model);
    }
}
