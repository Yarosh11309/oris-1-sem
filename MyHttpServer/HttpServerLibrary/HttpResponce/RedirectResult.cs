using System.Net;
using HttpServerLibrary.Core;
using HttpServerLibrary.Session;

namespace HttpServerLibrary.HttpResponce;

public class RedirectResult: IHttpResponseResult
{
    private readonly string _location;
    public RedirectResult(string location)
    {
        _location = location;
    }
 
    public void Execute(HttpListenerResponse context)
    {
        context.Redirect(_location);
        context.StatusCode = 302;
        context.OutputStream.Close();
    }

}