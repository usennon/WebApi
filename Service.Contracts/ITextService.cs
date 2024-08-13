using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface ITextService
    {
        string ToUpperCase(string str);

        string Concatenate(string str0, string str1);
    }
}
