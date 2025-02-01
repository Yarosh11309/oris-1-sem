using System.Data;
using Microsoft.Data.SqlClient;

namespace MyORMLibrary;

public class Test1Context<T> where T : class, new()
{
    private readonly IDbConnection _dbDbConnection;

    public Test1Context(IDbConnection dbConnection)
    {
        _dbDbConnection = dbConnection;
    }

    public T GetById(int id)
    {
        string query = $"SELECT * FROM {typeof(T).Name}s WHERE Id = @Id"; // Используем имя класса в качестве имени таблицы

        _dbDbConnection.Open();
        using (var command = _dbDbConnection.CreateCommand())
        {
            command.CommandText = query;
            
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Map(reader);
                    }
                }
            
        }

        return null;
    }

    private T Map(IDataReader reader)
    {
        var entity = new T();
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            if (reader[property.Name] != DBNull.Value)
            {
                property.SetValue(entity, reader[property.Name]);
            }
        }

        return entity;
    }


    /*
     * var parameter = command.CreateParameter();
            parameter.ParameterName = "@Id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

    */
}