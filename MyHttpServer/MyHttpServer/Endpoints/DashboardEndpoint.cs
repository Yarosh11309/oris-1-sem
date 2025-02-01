using HttpServerLibrary.Attributes;
using HttpServerLibrary.Core;
using HttpServerLibrary.HttpResponce;
using HttpServerLibrary.Session;
using Microsoft.Data.SqlClient;
using MyHttpServer.Models;
using MyORMLibrary;
using TemplateEngine;

namespace MyHttpServer.Endpoints;

internal class DashboardEndpoint: BaseEndpoint
{
    [Get("dashboard")]
    public IHttpResponseResult GetPage()
    {
        //if (!IsAuthorized(Context)) return Redirect("login");

        var dashboardTemplate = File.ReadAllText(@"Templates/Pages/Dashboard/index.html");
        //string connectionString =
        //    @"Data Source=localhost;Initial Catalog=master;User ID=sa;Password=P@ssw0rd;TrustServerCertificate=true;";

        // Получаем токен из куки
        //var token = Context.Request.Cookies.FirstOrDefault(c => c.Name == "session-token")?.Value;
        //if (string.IsNullOrEmpty(token) || !SessionStorage.ValidateToken(token))
        //{
        //    return Redirect("login");
        //}

        // Получаем ID пользователя из сессии
        //var userId = SessionStorage.GetUserId(token);
        //if (userId == null)
        //{
        //    return Redirect("login");
        //}

        // Загружаем данные пользователя из базы данных
        //var connection = new SqlConnection(connectionString);
        //var context = new ORMContext<User>(connection);
        //var user = context.ReadByAll($"Id = {userId.Value}").FirstOrDefault();

        //if (user == null)
        //{
        //    return Redirect("login");
        //}

        // Заменяем плейсхолдеры в шаблоне данными пользователя
        //dashboardTemplate = dashboardTemplate
        //    .Replace("{{name}}", user.Name)
        //    .Replace("{{last_name}}", user.Last_Name)
        //    .Replace("{{email}}", user.Email);
        

        return Html(dashboardTemplate);
    }

    [Get("movies/all")]
    public IHttpResponseResult GetPosterUrl()
    {
        string connectionString =
            @"Server=localhost; Database=DB; User Id=SA; Password=P@ssw0rd;TrustServerCertificate=true;";
        var dBcontext = new ORMContext<Movies>(new SqlConnection(connectionString));
        var movies = dBcontext.GetMovies();
        var data = PutDataToTemplate(movies);
        return Json(data);
    }
    public List<string> PutDataToTemplate(List<Movies> movies)
    {
        return movies.Select(CustomTemplator.GetCreateFilmsWithData).ToList();
    }

}