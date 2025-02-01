using HttpServerLibrary.HttpResponce;
using HttpServerLibrary.Session;

namespace HttpServerLibrary.Core;

/// <summary>
/// Базовый класс для всех конечных точек (Endpoints).
/// Предоставляет общий функционал для работы с HTTP-контекстом.
/// </summary>
public class BaseEndpoint
{
    /// <summary>
    /// Контекст HTTP-запроса и ответа, связанный с данным Endpoint.
    /// </summary>
    protected HttpRequestContext Context { get; private set; }

    /// <summary>
    /// Устанавливает HTTP-контекст для текущей конечной точки.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса и ответа.</param>
    internal void SetContext(HttpRequestContext context)
    {
        Context = context;
    }
    
    protected IHttpResponseResult Html(string responseText) => new HtmlResult(responseText);

    protected IHttpResponseResult Json(object data) => new JsonResult(data);
    
    protected IHttpResponseResult Redirect(string location) => new RedirectResult(location);
    
    public bool IsAuthorized(HttpRequestContext context)
    {
        // Проверка наличия Cookie с session-token
        if (context.Request.Cookies.Any(c=> c.Name == "session-token"))
        {
            var cookie = context.Request.Cookies["session-token"];
            return SessionStorage.ValidateToken(cookie.Value);
        }
         
        return false;
    }
}