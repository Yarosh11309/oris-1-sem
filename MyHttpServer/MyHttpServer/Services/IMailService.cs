namespace MyHttpServer.Services;

/// <summary>
/// Интерфейс для сервиса отправки электронной почты.
/// Определяет метод для асинхронной отправки писем.
/// </summary>
internal interface IMailService
{
    /// <summary>
    /// Асинхронно отправляет письмо с указанным получателем, темой и содержимым.
    /// </summary>
    /// <param name="recipientEmail">Email адрес получателя.</param>
    /// <param name="subject">Тема письма.</param>
    /// <param name="body">Содержимое письма (может быть в формате HTML).</param>
    Task SendAsync(string recipientEmail, string subject, string body);
}