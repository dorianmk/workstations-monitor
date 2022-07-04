using DataTransfer.Interfaces;

namespace Common.DataTransfer.DataPackets.AdminClient
{
    public class LoginAnswerPacket : IData
    {
        public bool IsValid { get; private set; }

        public LoginAnswerPacket(bool isValid)
        {
            IsValid = isValid;
        }
    }

    public class LoginRequestPacket : IData
    {
        public string Login { get; private set; }
        public string Password { get; private set; }

        public LoginRequestPacket(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }

    public class LogoutPacket : IData
    {

    }
}
