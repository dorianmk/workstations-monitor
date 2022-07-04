using Common.Interfaces;
using DataTransfer.Interfaces;

namespace WorkstationService.Entry.Components
{
    internal class SendToServerAction : IAction<IData>
    {
        private IClientConnection Connection { get; }

        public SendToServerAction(IClientConnection connection)
        {
            Connection = connection;
        }

        public void Do(IData item)
        {
            Connection.Server.Write(item);
        }
    }
}
