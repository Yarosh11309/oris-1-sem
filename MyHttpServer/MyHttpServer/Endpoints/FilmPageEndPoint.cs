using System.Text;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Core;
using HttpServerLibrary.HttpResponce;
using Microsoft.Data.SqlClient;
using MyHttpServer.Core;
using MyHttpServer.Models;
using MyORMLibrary;
using TemplateEngine;


namespace MyHttpServer.EndPoints;

internal class EmailSenderEndpoint : BaseEndpoint
{
    [Get("movies")]
    public IHttpResponseResult GetFilmPage(int film_id)
    {
        var file = File.ReadAllText(@"Templates/Pages/film_page/index.html");
        string connectionString =
            @"Server=localhost; Database=DB; User Id=SA; Password=P@ssw0rd;TrustServerCertificate=true;";
        var dBcontext = new ORMContext<Movies>(new SqlConnection(connectionString));
        var movies = dBcontext.ReadById(film_id);
        var result = CustomTemplator.GetFilmPageWithData(movies, file);
        return Html(result);
    }

}