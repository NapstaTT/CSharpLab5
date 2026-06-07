///FIXME:
/// - Добавлена XML-документация
/// - Приведены скобки к стилю Allman
/// - Удалены лишние пустые строки
/// - Параметры конструктора приведены к camelCase

namespace lab5.Data.Models
{
    /// <summary>
    /// Представляет футбольный клуб.
    /// </summary>
    public class Club
    {
        /// <summary>
        /// Уникальный идентификатор клуба.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название клуба.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор страны, к которой относится клуб.
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Создаёт пустой объект клуба.
        /// </summary>
        public Club()
        {
        }

        /// <summary>
        /// Создаёт объект клуба с указанными параметрами.
        /// </summary>
        public Club(int id, string name, int countryId)
        {
            Id = id;
            Name = name ?? string.Empty;
            CountryId = countryId;
        }

        /// <summary>
        /// Возвращает строковое представление объекта.
        /// </summary>
        public override string ToString()
        {
            return $"{Id}: {Name} (Country: {CountryId})";
        }
    }
}
