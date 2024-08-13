using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IMathService
    {
        public int Sum(int a, int b);

        public int Sub(int a, int b);
    }
}
