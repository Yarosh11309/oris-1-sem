using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpServerLibrary.Configuration;

/// <summary>
/// Синглтон-класс для загрузки и хранения конфигурации приложения.
/// </summary>
public sealed class AppConfig
{
    private static readonly object _lock = new();
    private static AppConfig? _instance;

    /// <summary>
    /// Единственный экземпляр конфигурации приложения.
    /// Загружается при первом обращении.
    /// </summary>
    public static AppConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = LoadConfiguration();
                    }
                }
            }
            return _instance;
        }
    }
    
    public string Domain { get; }
    public uint Port { get; }
    public string StaticDirectoryPath { get; }
    private AppConfig()
    {
        Domain = "localhost";
        Port = 6529;
        StaticDirectoryPath = @"public/";
    }

    /// <summary>
    /// Конструктор для десериализации JSON.
    /// </summary>
    [JsonConstructor]
    public AppConfig(string domain, uint port, string staticDirectoryPath)
    {
        Domain = domain ?? "localhost";
        Port = port > 0 ? port : 6529;
        StaticDirectoryPath = staticDirectoryPath ?? @"public/";
    }

    /// <summary>
    /// Загрузка конфигурации из файла config.json.
    /// Если файл отсутствует или некорректен, используются значения по умолчанию.
    /// </summary>
    /// <returns>Экземпляр <see cref="AppConfig"/>.</returns>
    private static AppConfig LoadConfiguration()
    {
        const string configPath = "config.json";

        if (File.Exists(configPath))
        {
            try
            {
                var fileConfig = File.ReadAllText(configPath);
                return JsonSerializer.Deserialize<AppConfig>(fileConfig) ?? new AppConfig();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки конфигурации из файла: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"Файл конфигурации '{configPath}' не найден. Используются значения по умолчанию.");
        }

        return new AppConfig();
    }
}