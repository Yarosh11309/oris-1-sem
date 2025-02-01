using System.Net;
using System.Net.Mail;
using System.Text;
using MyHttpServer.Models;

namespace MyHttpServer.Services;

/// <summary>
/// Сервис для отправки писем по электронной почте.
/// Реализует функциональность отправки асинхронных сообщений.
/// </summary>
internal class MailService : IMailService
{
    /// <summary>
    /// Асинхронно отправляет письмо с указанным получателем, темой и содержимым.
    /// </summary>
    /// <param name="recipientEmail">Email получателя.</param>
    /// <param name="subject">Тема письма.</param>
    /// <param name="body">Содержимое письма (может быть в формате HTML).</param>
    public async Task SendAsync(string recipientEmail, string subject, string body)
    {
        try
        {
            // Создание и настройка сообщения
            using (var message = new MailMessage())
            {
                message.From = new MailAddress("g.arthur.a@yandex.ru", "art"); // Адрес отправителя
                message.To.Add(new MailAddress(recipientEmail)); // Добавляем адрес получателя
                message.Subject = subject; // Устанавливаем тему письма
                message.Body = body; // Устанавливаем текст письма
                message.IsBodyHtml = true; // Указываем, что письмо поддерживает HTML
                message.BodyEncoding = Encoding.UTF8; // Кодировка текста письма
                message.SubjectEncoding = Encoding.UTF8; // Кодировка темы письма

                Console.WriteLine("Создано письмо.");

                // Настройка безопасности соединения
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // Настройка SMTP-клиента для отправки письма
                using (var smtp = new SmtpClient("smtp.yandex.ru", 587)) // SMTP-сервер Yandex и порт
                {
                    smtp.Credentials = new NetworkCredential("g.arthur.a@yandex.ru", "ayefwbrefvczzpzp"); // Логин и пароль отправителя
                    smtp.EnableSsl = true; // Включение SSL для безопасности

                    // Асинхронная отправка письма
                    await smtp.SendMailAsync(message);
                    Console.WriteLine("Письмо отправлено.");
                }
            }
        }
        catch (Exception ex)
        {
            // Логирование ошибок отправки письма
            Console.WriteLine($"Ошибка при отправке письма: {ex.Message}");
        }
    }
}
