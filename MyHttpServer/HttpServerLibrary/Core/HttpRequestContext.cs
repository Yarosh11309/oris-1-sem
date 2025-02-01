using System.Net;

namespace HttpServerLibrary.Core;

/// <summary>
/// Класс, представляющий контекст HTTP-запроса.
/// Объединяет запрос и ответ для удобного использования в обработчиках.
/// </summary>
public class HttpRequestContext
{
    /// <summary>
    /// HTTP-запрос, полученный от клиента.
    /// </summary>
    public HttpListenerRequest Request { get; set; }
    
    /// <summary>
    /// HTTP-ответ, отправляемый клиенту.
    /// </summary>
    public HttpListenerResponse Response { get; set; }
    
    /// <summary>
    /// Конструктор, инициализирующий контекст на основе объекта <see cref="HttpListenerContext"/>.
    /// </summary>
    /// <param name="context">Контекст запроса, содержащий запрос и ответ.</param>
    public HttpRequestContext(HttpListenerContext context)
    {
        Request = context.Request;
        Response = context.Response;
    }
}