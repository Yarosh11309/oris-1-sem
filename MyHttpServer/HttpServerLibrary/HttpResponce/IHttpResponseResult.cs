using System.Net;
using HttpServerLibrary.Core;

namespace HttpServerLibrary.HttpResponce
{
    public interface IHttpResponseResult
    {
        void Execute(HttpListenerResponse response);
    }
}