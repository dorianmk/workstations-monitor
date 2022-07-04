
namespace ServerService.Common.Hashing
{
    internal class BCryptHashing : IHashing
    {
        public string GetHash(string text) => BCrypt.Net.BCrypt.HashPassword(text);

        public bool Verify(string text, string hash) => BCrypt.Net.BCrypt.Verify(text, hash);

    }
}
