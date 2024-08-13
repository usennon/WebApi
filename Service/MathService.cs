using Service.Contracts;

namespace Service
{
    internal sealed class MathService: IMathService
    {
        public int Sum(int a, int b) => a + b;

        public int Sub(int a, int b) => a - b;
    }
}
