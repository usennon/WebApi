using Service.Contracts;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Service
{
    internal sealed class TextService : ITextService
    {
        public string ToUpperCase(string str) => str.ToUpper();

        public string Concatenate(string str1, string str2)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(str1);
            sb.Append(str2);
            return sb.ToString();
        }
    }
}
