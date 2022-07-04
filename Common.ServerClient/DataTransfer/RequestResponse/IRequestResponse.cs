using System;

namespace Common.DataTransfer.RequestResponse
{
    public interface IRequestResponse
    {
        bool DoCallbackOnResponse(IDataWithId request, Action<IDataWithId> responseCallback);
        bool TryGetResponse<T>(IDataWithId request, out T response, TimeSpan timeout) where T : IDataWithId;
    }
}
