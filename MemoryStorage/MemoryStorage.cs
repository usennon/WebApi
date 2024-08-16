using MemoryStorage.Interfaces;
using System.Text;

namespace MemoryStorage
{
    public class CalculationMemoryStorage : IMemoryStorage
    {
        private int _currentTotal = 0;

        private string _concatenated = string.Empty;

        public void AddResult(int result)
        {
            _currentTotal = result;
        }

        public int GetCurrentTotal()
        {
            return _currentTotal;
        }

        public string GetCurrentString()
        {
            return _concatenated;
        }

        public void Clear()
        {
            _currentTotal = 0;
            _concatenated = string.Empty;
        }

        public string Concatenate(string str1, string str2)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(str1);
            sb.Append(str2);
            sb.Append(_concatenated);
            _concatenated = sb.ToString();
            return _concatenated;
        }
    }

}
