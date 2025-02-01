// using System.Net;
// using System.Text;
// using System.Web;
// using MyHttpServer.Models;
// using MyHttpServer.Services;
//
// namespace MyHttpServer.Endpoints;
//
// internal class HomeWorkEndpoints : BaseEndpoint
// {
//     private readonly MailService _mailService;
//
//     public HomeWorkEndpoints()
//     {
//         _mailService = new MailService();
//     }
//
//     // Обработка GET-запроса для отображения формы
//     [Get("send-home-work")]
//     public void ShowForm()
//     {
//         string filePath = Path.Combine(AppConfig.Instance.StaticDirectoryPath, "index.html");
//
//         if (File.Exists(filePath))
//         {
//             string html = File.ReadAllText(filePath);
//
//             Context.Response.ContentType = "text/html";
//             byte[] responseBytes = Encoding.UTF8.GetBytes(html);
//             Context.Response.ContentLength64 = responseBytes.Length;
//
//             using var output = Context.Response.OutputStream;
//             output.Write(responseBytes, 0, responseBytes.Length);
//         }
//         else
//         {
//             SendErrorResponse("Файл формы не найден.", HttpStatusCode.NotFound);
//         }
//     }
//
//     // Обработка POST-запроса для отправки данных
//     [Post("send-home-work")]
//     public void ProcessSubmission()
//     {
//         try
//         {
//             var user = ExtractUserFromRequest();
//             string homeworkDetails = "Ваше домашнее задание успешно отправлено.";
//
//             // Отправка сообщения
//             _mailService.SendAsync(user.Login, "Домашнее задание", homeworkDetails).Wait();
//
//             // Ответ клиенту
//             SendTextResponse("Домашнее задание отправлено на вашу почту.");
//         }
//         catch (Exception ex)
//         {
//             SendErrorResponse($"Ошибка при обработке: {ex.Message}", HttpStatusCode.InternalServerError);
//         }
//     }
//
//     // Метод для извлечения данных пользователя из запроса
//     private User ExtractUserFromRequest()
//     {
//         using var reader = new StreamReader(Context.Request.InputStream);
//         string body = reader.ReadToEnd();
//         var data = HttpUtility.ParseQueryString(body);
//
//         return new User
//         {
//             Login = data["Login"] ?? throw new ArgumentException("Не указан Login."),
//             Password = data["Password"] ?? throw new ArgumentException("Не указан Password.")
//         };
//     }
//
//     // Метод для отправки текстового ответа клиенту
//     private void SendTextResponse(string responseText, HttpStatusCode statusCode = HttpStatusCode.OK)
//     {
//         Context.Response.StatusCode = (int)statusCode;
//         byte[] responseBytes = Encoding.UTF8.GetBytes(responseText);
//
//         using var output = Context.Response.OutputStream;
//         output.Write(responseBytes, 0, responseBytes.Length);
//     }
//
//     // Метод для отправки ошибки
//     private void SendErrorResponse(string errorMessage, HttpStatusCode statusCode)
//     {
//         Context.Response.StatusCode = (int)statusCode;
//         SendTextResponse(errorMessage, statusCode);
//     }
// }
