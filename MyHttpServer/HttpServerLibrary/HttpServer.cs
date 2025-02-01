using System.Net;
using System.Web;
using HttpServerLibrary.Core;
using HttpServerLibrary.Handlers;

namespace HttpServerLibrary;

/// <summary>
/// Класс HTTP-сервера для обработки запросов.
/// Поддерживает статические файлы и конечные точки (endpoints).
/// </summary>
public sealed class HttpServer
{
    // Слушатель HTTP-запросов
    private readonly HttpListener _listener;
    
    // Обработчик статических файлов
    private readonly StaticFilesHandler _staticFilesHandler;

    // Обработчик конечных точек
    private readonly EndpointsHandler _endpointsHandler;

    /// <summary>
    /// Конструктор сервера.
    /// Инициализирует слушатель и добавляет префиксы.
    /// </summary>
    /// <param name="prefixes">Список URL-префиксов для прослушивания.</param>
    public HttpServer(string[] prefixes)
    {
        _listener = new HttpListener();

        // Добавление всех указанных префиксов
        foreach (string prefix in prefixes)
        {
            _listener.Prefixes.Add(prefix); // Префикс для прослушивания запросов
        }
        
        _staticFilesHandler = new StaticFilesHandler(); // Инициализация обработчика статических файлов
        _endpointsHandler = new EndpointsHandler();     // Инициализация обработчика конечных точек
    }

    /// <summary>
    /// Запускает сервер и начинает обработку запросов.
    /// </summary>
    public async Task StartAsync()
    {
        _listener.Start();
        Console.WriteLine("Сервер запущен и ожидает запросов");

        while (_listener.IsListening)
        {
            // Получение контекста запроса
            var context = await _listener.GetContextAsync();

            // Преобразование в контекст HTTP-запроса
            var httpRequestContext = new HttpRequestContext(context);

            // Обработка запроса
            await ProcessRequestAsync(httpRequestContext);
        }
    }
    
    /// <summary>
    /// Обрабатывает HTTP-запрос с использованием цепочки обработчиков.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    private async Task ProcessRequestAsync(HttpRequestContext context)
    {
        // Устанавливаем цепочку обработчиков: StaticFilesHandler -> EndpointsHandler
        _staticFilesHandler.Successor = _endpointsHandler;

        // Передаем запрос первому обработчику
        _staticFilesHandler.HandleRequest(context);
    }     

    // /// <summary>
    // /// Получает данные из POST-запроса и преобразует их в объект пользователя.
    // /// </summary>
    // /// <param name="request">HTTP-запрос.</param>
    // /// <returns>Объект <see cref="User"/>, заполненный данными из запроса.</returns>
    // private async Task<User> GetPostData(HttpListenerRequest request)
    // {
    //     // Чтение содержимого тела запроса
    //     using var reader = new StreamReader(request.InputStream);
    //     string body = await reader.ReadToEndAsync();
    //
    //     // Распарсим данные
    //     var data = HttpUtility.ParseQueryString(body);
    //
    //     // Создаем и заполняем объект User
    //     var user = new User
    //     {
    //         Login = data["Login"], // Логин пользователя
    //         Password = data["Password"] // Пароль пользователя
    //     };
    //
    //     return user;
    // }

    /// <summary>
    /// Останавливает сервер и завершает обработку запросов.
    /// </summary>
    public void Stop()
    {
        _listener.Stop();
        Console.WriteLine("Сервер остановлен");
    }
}
