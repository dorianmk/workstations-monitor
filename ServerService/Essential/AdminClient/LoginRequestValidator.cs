using Common.DataTransfer.DataPackets.AdminClient;
using Database.Interfaces;
using ServerService.Common.Hashing;

namespace ServerService.Essential.AdminClient
{
    internal interface ILoginRequestValidator
    {
        bool IsValid(LoginRequestPacket loginRequest);
    }

    internal class LoginRequestValidator : ILoginRequestValidator
    {
        private IDatabase Database { get; }
        private IHashing Hashing { get; }

        public LoginRequestValidator(IDatabase database, IHashing hashing)
        {
            Database = database;
            Hashing = hashing;
        }

        public bool IsValid(LoginRequestPacket loginRequest)
        {
            var isValid = false;
            var user = Database.Users.FindFirst(x => x.Login.Equals(loginRequest.Login));
            if (user != null)
                isValid = Hashing.Verify(loginRequest.Password, user.PasswordHash);
            return isValid;
        }
    }
}
