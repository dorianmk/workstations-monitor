
namespace ServerService.Common.Hashing
{
    internal interface IHashing
    {
        string GetHash(string text);
        bool Verify(string text, string hash);
    }
}
