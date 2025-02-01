using HttpServerLibrary.Attributes;
using HttpServerLibrary.Core;
using HttpServerLibrary.HttpResponce;
using HttpServerLibrary.Session;
using Microsoft.Data.SqlClient;
using MyHttpServer.Models;
using MyORMLibrary;

namespace MyHttpServer.Endpoints;

internal class UserProfileEndpoint : BaseEndpoint
{
    [Get("userprofile")]
    public IHttpResponseResult GetPage()
    {
        if (!IsAuthorized(Context)) return Redirect("login");

        var profileTemplate = File.ReadAllText(@"Templates/Pages/UserProfile/profile.html");
        string connectionString =
            @"Data Source=localhost;Initial Catalog=master;User ID=sa;Password=P@ssw0rd;TrustServerCertificate=true;";

        // Получаем токен из куки
        var token = Context.Request.Cookies.FirstOrDefault(c => c.Name == "session-token")?.Value;
        if (string.IsNullOrEmpty(token) || !SessionStorage.ValidateToken(token))
        {
            return Redirect("login");
        }

        // Получаем ID пользователя из сессии
        var userId = SessionStorage.GetUserId(token);
        if (userId == null)
        {
            return Redirect("login");
        }

        // Загружаем данные пользователя из базы данных
        var connection = new SqlConnection(connectionString);
        var context = new ORMContext<User>(connection);
        var user = context.ReadByAll($"Id = {userId.Value}").FirstOrDefault();

        if (user == null)
        {
            return Redirect("login");
        }

        // Заменяем плейсхолдеры в шаблоне данными пользователя
        profileTemplate = profileTemplate
            .Replace("{{name}}", user.Name)
            .Replace("{{last_name}}", user.Last_Name)
            .Replace("{{email}}", user.Email)
            .Replace("{{adress}}", user.Address)
            .Replace("{{profession}}", user.Profession)
            .Replace("{{aboutMe}}", user.AboutMe);

        return Html(profileTemplate);
    }
}
