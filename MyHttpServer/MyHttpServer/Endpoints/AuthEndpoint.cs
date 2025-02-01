using System.Net;
using System.Web;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Configuration;
using HttpServerLibrary.Core;
using HttpServerLibrary.HttpResponce;
using HttpServerLibrary.Session;
using Microsoft.Data.SqlClient;
using MyHttpServer.Models;
using MyORMLibrary;

namespace MyHttpServer.Endpoints;

internal class AuthEndpoint : BaseEndpoint
{
    [Get("login")]
    public IHttpResponseResult GetLogin()
    {
        //if (IsAuthorized(Context)) return Redirect("dashboard");

        var file = File.ReadAllText(@"Templates/Pages/Auth/login.html");
        return Html(file);
    }

    [Post("login")]
    public IHttpResponseResult PostLogin(string email, string password)
    {
        string connectionString =
            @"Data Source=localhost;Initial Catalog=master;User ID=sa;Password=P@ssw0rd;TrustServerCertificate=true;";
        var connection = new SqlConnection(connectionString);
        var context = new ORMContext<User>(connection);

        // Проверяем, существует ли пользователь
        var user = context.ReadByAll($"Email = '{email}' AND Password = '{password}'").FirstOrDefault();
        
        
        if (user == null)
        {
            Console.WriteLine($"Login failed: No user found with email '{email}' and password '{password}'");
            return Redirect("login");
        }

        Console.WriteLine($"Login succeeded: User '{user.Email}' logged in");

        string token = Guid.NewGuid().ToString();
        Cookie cookie = new Cookie("session-token", token);
        Context.Response.SetCookie(cookie);
        SessionStorage.SaveSession(token, user.Id);

        return Redirect("dashboard");
    }


}