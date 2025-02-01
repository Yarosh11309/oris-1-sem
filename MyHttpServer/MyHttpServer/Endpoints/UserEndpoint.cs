using System.Data;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Core;
using HttpServerLibrary.HttpResponce;
using Microsoft.Data.SqlClient;
using MyHttpServer.Models;
using MyORMLibrary;

// var connectionString = @"Data Source=localhost;Initial Catalog=master;User ID=sa;Password=P@ssw0rd;TrustServerCertificate=true;";
namespace MyHttpServer.Endpoints;

internal class UserEndpoint: BaseEndpoint
{
    [Get("users")]
    public IHttpResponseResult GetUsers()
    {
        var dbConnection = @"Data Source=localhost;Initial Catalog=master;User ID=sa;Password=P@ssw0rd;TrustServerCertificate=true;";
        var context = new ORMContext<User>(new SqlConnection(dbConnection));

        var users = context.ReadByAll();

        return Json(users);
    }

}