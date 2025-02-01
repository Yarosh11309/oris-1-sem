namespace MyHttpServer.Models;

/// <summary>
/// Модель пользователя, содержащая все данные из таблицы Users.
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор пользователя (первичный ключ).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    public string Last_Name { get; set; } = string.Empty;

    /// <summary>
    /// Адрес пользователя.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Электронная почта пользователя.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Телефон пользователя.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Указывает, заблокирован ли пользователь (true - заблокирован, false - нет).
    /// </summary>
    public bool IfBlocked { get; set; }
    
    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Профессия пользователя.
    /// </summary>
    public string Profession { get; set; } = string.Empty;

    /// <summary>
    /// Описание пользователя.
    /// </summary>
    public string AboutMe { get; set; } = string.Empty;
}