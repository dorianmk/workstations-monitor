using DataTransfer.Interfaces;
using System.Collections.Generic;

namespace Common.DataTransfer.DataPackets.AdminClient
{
    public class GetUsersPacket : IData
    {
        public List<UserDTO> Users { get; set; }
    }

    public class UserDTO
    {
        public string Id { get; set; }
        public string Login { get; set; }
    }

    public class ChangePasswordPacket : IData
    {
        public string Id { get; private set; }
        public string Password { get; private set; }

        public ChangePasswordPacket(string id, string password)
        {
            Id = id;
            Password = password;
        }
    }
}
