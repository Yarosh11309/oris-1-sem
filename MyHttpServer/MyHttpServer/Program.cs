using System.Net;
using HttpServerLibrary;
using HttpServerLibrary.Configuration;


namespace MyHttpServer;

/// <summary>
/// Главный класс программы, отвечающий за инициализацию и запуск HTTP-сервера.
/// </summary>
class Program
{
    #region 
    //++ TODO: [Архитектурно] Необходимо  из запроса передавать параметры: если это Get -> query, если Post -> formData в метод соответствующий в Endpoints
    //++ TODO: [Архитектурно] Превратить AppConfig в синглтон, не меняя его текущей логики работы
    //++ TODO: [Архитектурно] EndpointsHandler. При регистрации роутингов если существует уже такое роутинг + метод выкидывать в лог ошибку об этом и не запускать сервер
    // TODO: Добавить в проект HomeWorkEndpoints. Рeализовать в нем метод (роутинг "send-home-work") который вызывает отправку сообщения
    // с вашим выполненным ДЗ на почту которое приходит в параметрах запроса со страницы EA/login (метод Get + Post)

    // TODO*: [Архитектурно] Необходимо доработать вызов метода в endpoints таким образом, чтобы не было необходимости превращать в каждом методе все в байты
    #endregion
    
    // TODO 7 баз данных за неделю
    // TODO SQL за час видос
    // TODO почитать metantit SQL, Что такое creat read update delete

    
    /// <summary>
    /// Точка входа в приложение.
    /// Инициализирует настройки сервера и запускает его.
    /// </summary>
    static async Task Main()
    {
        // Формирование префиксов на основе конфигурации приложения
        var prefixes = new[] { $"http://{AppConfig.Instance.Domain}:{AppConfig.Instance.Port}/" };
        
        // Создание HTTP-сервера с указанными префиксами
        var server = new HttpServer(prefixes);

        // Асинхронный запуск сервера
        await server.StartAsync();
    }
}

