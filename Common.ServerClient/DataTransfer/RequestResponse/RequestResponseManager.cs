using DataTransfer.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Common.DataTransfer.RequestResponse
{
    public class RequestResponseManager : IRequestResponse
    {
        private ConcurrentDictionary<Guid, Action<IDataWithId>> CallbacksCache { get; }
        private IDataTwoWay Connection { get; }

        public RequestResponseManager(IDataTwoWay connection)
        {
            Connection = connection;
            Connection.OnRead += Client_OnRead;
            CallbacksCache = new ConcurrentDictionary<Guid, Action<IDataWithId>>();
        }

        private void Client_OnRead(object sender, IData data)
        {
            if (data is IDataWithId dataWithId)
            {
                if (CallbacksCache.TryRemove(dataWithId.Id, out var callback))
                {
                    callback(dataWithId);
                }
            }
        }

        public bool DoCallbackOnResponse(IDataWithId requestPacket, Action<IDataWithId> responseCallback)
        {
            if (requestPacket.Id == default(Guid))
                throw new ArgumentException("GUID not set");
            else if (!CallbacksCache.TryAdd(requestPacket.Id, responseCallback))
                throw new ArgumentException("GUID already exists");
            else
                return Connection.Write(requestPacket);
        }

        public bool TryGetResponse<T>(IDataWithId request, out T response, TimeSpan timeout)
            where T : IDataWithId
        {
            var gotResponseEvent = new ManualResetEvent(false);           
            T responseTemp = default(T);
            var sent = DoCallbackOnResponse(request, (res) => { responseTemp = (T)res;
                                                                gotResponseEvent.Set(); });
            if (sent)
            {
                var gotResponse = gotResponseEvent.WaitOne(timeout);
                if (gotResponse)
                {
                    response = responseTemp;
                    return true;
                }
            }
            response = default(T);
            return false;
        }
    }


}
