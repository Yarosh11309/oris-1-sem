using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Data.SqlClient;
using MyHttpServer.Models;

namespace MyORMLibrary;

public class ORMContext<T> where T : class, new()
{
    private readonly IDbConnection _dbConnection;

    public ORMContext(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public List<Movies> GetMovies()
    {
        var result = new List<Movies>();
        string query = "SELECT id, film_name, release_year, rating, description, poster_url, card_url  FROM movies";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var movies = new Movies
                    {
                        Id = reader.GetInt32(0),
                        film_name = reader.GetString(1),
                        release_year = reader.GetInt32(2),
                        rating = reader.GetDouble(3),
                        description = reader.GetString(4),
                        poster_url = reader.GetString(5),
                        card_url = reader.GetString(6),
                    };
                    result.Add(movies);
                }
            }
            _dbConnection.Close();
        }
        return result;
    }

    public void Create(T entity)
    {
        var properties = entity.GetType().GetProperties()
            .Where(p => p.Name != "Id")
            .ToList();

        var columns = string.Join(", ", properties.Select(p => p.Name));
        var values = string.Join(", ", properties.Select(p => "@" + p.Name));

        var query = $"INSERT INTO {typeof(T).Name}s ({columns}) VALUES ({values})";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;

            foreach (var property in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@" + property.Name;
                parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }

            EnsureConnectionOpen();
            command.ExecuteNonQuery();
            EnsureConnectionClosed();
        }
    }

    public T ReadById(int id)
    {
        var tableName = typeof(T).Name;
        var sql = $"SELECT * FROM {tableName} WHERE Id = @id";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = sql;

            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            EnsureConnectionOpen();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }

            EnsureConnectionClosed();
        }

        return null;
    }

    public IEnumerable<T> ReadByAll(string filter = null)
    {
        var result = new List<T>();
        var tableName = typeof(T).Name + "s";
        var sql = $"SELECT * FROM {tableName}";

        if (!string.IsNullOrEmpty(filter))
        {
            sql += $" WHERE {filter}";
        }

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = sql;

            EnsureConnectionOpen();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(Map(reader));
                }
            }

            EnsureConnectionClosed();
        }

        return result;
    }

    public void Update(int id, T entity)
    {
        var properties = typeof(T).GetProperties()
            .Where(p => p.Name != "Id")
            .ToList();

        var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
        var query = $"UPDATE {typeof(T).Name}s SET {setClause} WHERE Id = @id";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;

            foreach (var property in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@" + property.Name;
                parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            EnsureConnectionOpen();
            command.ExecuteNonQuery();
            EnsureConnectionClosed();
        }
    }

    public void Delete(int id)
    {
        var query = $"DELETE FROM {typeof(T).Name}s WHERE Id = @id";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            EnsureConnectionOpen();
            command.ExecuteNonQuery();
            EnsureConnectionClosed();
        }
    }

    private T Map(IDataReader reader)
    {
        var entity = new T();
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
            {
                property.SetValue(entity, reader[property.Name]);
            }
        }

        return entity;
    }

    public T FirstOrDefault(Expression<Func<T, bool>> predicate)
    {
        var query = BuildQuery(predicate);

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query.Item1;
            foreach (var parameter in query.Item2)
            {
                var dbParameter = command.CreateParameter();
                dbParameter.ParameterName = parameter.Key;
                dbParameter.Value = parameter.Value;
                command.Parameters.Add(dbParameter);
            }

            EnsureConnectionOpen();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            EnsureConnectionClosed();
        }
        return null;
    }

    private T ExecuteQuerySingle(string query)
    {
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            EnsureConnectionOpen();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            EnsureConnectionClosed();
        }

        return null;
    }

    private IEnumerable<T> ExecuteQueryMultiple(string query)
    {
        var results = new List<T>();
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            EnsureConnectionOpen();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    results.Add(Map(reader));
                }
            }
            EnsureConnectionClosed();
        }
        return results;
    }

    private Tuple<string, Dictionary<string, object>> BuildQuery(Expression<Func<T, bool>> predicate)
    {
        var tableName = typeof(T).Name + "s";
        var parameters = new Dictionary<string, object>();
        var whereClause = BuildWhereClause(predicate.Body, parameters);

        var query = $"SELECT * FROM {tableName} WHERE {whereClause}";
        return new Tuple<string, Dictionary<string, object>>(query, parameters);
    }

    private string BuildWhereClause(Expression expression, Dictionary<string, object> parameters)
    {
        if (expression is BinaryExpression binaryExpression)
        {
            var left = BuildWhereClause(binaryExpression.Left, parameters);
            var right = BuildWhereClause(binaryExpression.Right, parameters);
            var operatorString = binaryExpression.NodeType switch
            {
                ExpressionType.Equal => "=",
                ExpressionType.NotEqual => "<>",
                ExpressionType.GreaterThan => ">",
                ExpressionType.GreaterThanOrEqual => ">=",
                ExpressionType.LessThan => "<",
                ExpressionType.LessThanOrEqual => "<=",
                ExpressionType.AndAlso => "AND",
                ExpressionType.OrElse => "OR",
                _ => throw new NotSupportedException($"Operator {binaryExpression.NodeType} is not supported")
            };

            return $"{left} {operatorString} {right}";
        }
        else if (expression is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }
        else if (expression is ConstantExpression constantExpression)
        {
            var parameterName = $"@p{parameters.Count}";
            parameters.Add(parameterName, constantExpression.Value);
            return parameterName;
        }
        else
        {
            throw new NotSupportedException($"Expression type {expression.GetType().Name} is not supported");
        }
    }

    private void EnsureConnectionOpen()
    {
        if (_dbConnection.State != ConnectionState.Open)
        {
            _dbConnection.Open();
        }
    }

    private void EnsureConnectionClosed()
    {
        if (_dbConnection.State != ConnectionState.Closed)
        {
            _dbConnection.Close();
        }
    }
}
