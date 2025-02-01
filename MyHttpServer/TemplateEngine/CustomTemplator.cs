using System.Reflection;
using MyHttpServer.Core.Templaytor;
using MyHttpServer.Models;

namespace TemplateEngine
{
    
    public class CustomTemplator : ICustomTemplator
    {
        public static string GetFilmPageWithData(Movies movies, string template)
        {
            template = template.Replace("{{film_name}}", movies.film_name);
            template = template.Replace("{{release_year}}", movies.release_year.ToString());
            template = template.Replace("{{rating}}", movies.rating.ToString());
            template = template.Replace("{{description}}", movies.description);
            template = template.Replace("{{poster_url}}", movies.poster_url);
            return template;
        }
        public static string GetCreateFilmsWithData(Movies movies)
        {
            return $@"<a href=""movies?film_id={movies.Id}""><img src=""{movies.card_url}"" alt=""T-34 Movie Poster"">
        <div class=""movie-title"">{movies.film_name}</div>
        <div class=""age-rating"">16+</div></a>";
        }
        // Метод для замены плейсхолдера {name} в шаблоне на переданное имя
        public string GetHtmlByTemplate(string template, string name)
        {
            return template.Replace("{name}", name);
        }

        // Метод для замены плейсхолдеров в шаблоне на значения свойств переданного объекта
        public string GetHtmlByTemplate<T>(string template, T obj)
        {
            // Если шаблон пустой или объект равен null, возвращаем исходный шаблон
            if (string.IsNullOrEmpty(template) || obj == null)
                return template;

            // Получаем все публичные свойства объекта
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Проходим по каждому свойству объекта
            foreach (var property in properties)
            {
                var propertyName = property.Name.ToLower(); // Название плейсхолдера в шаблоне: {name}, {login} и т.д.
                var propertyValue = property.GetValue(obj)?.ToString() ?? string.Empty;

                // Заменяем плейсхолдер в шаблоне на значение свойства
                template = template.Replace($"{{{propertyName}}}", propertyValue);
            }

            // Обработка условного шаблона вида "if(gender){...}else{...}"
            if (template.Contains("if(gender)"))
            {
                // Находим свойство gender в объекте
                var genderProperty = properties.FirstOrDefault(p => p.Name.Equals("gender", StringComparison.OrdinalIgnoreCase));
                if (genderProperty != null)
                {
                    // Проверяем, является ли значение свойства true (мужской пол)
                    bool isMale = bool.TryParse(genderProperty.GetValue(obj)?.ToString(), out bool gender) && gender;
                    // Разделяем шаблон на части: если условие выполняется и если нет
                    string[] parts = template.Split(new string[] { "if(gender)", "else" }, StringSplitOptions.None);

                    // Если шаблон разделился корректно, выбираем нужную часть
                    if (parts.Length == 3)
                    {
                        template = isMale ? parts[1] : parts[2];
                    }
                }
            }

            return template;
        }
    }
}