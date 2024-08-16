namespace MemoryStorage.Interfaces
{
    public interface IMemoryStorage
    {
        void AddResult(int result);

        public string Concatenate(string str1, string str2);

        int GetCurrentTotal();

        void Clear();
    }
}
