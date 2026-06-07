///FIXME:
/// - Добавлена XML-документация
/// - Приведены скобки к стилю Allman
/// - Удалены лишние пустые строки

namespace lab5.Data.Models
{
    /// <summary>
    /// Представляет страну.
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Уникальный идентификатор страны.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название страны.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Создаёт пустой объект страны.
        /// </summary>
        public Country()
        {
        }

        /// <summary>
        /// Создаёт объект страны с указанными параметрами.
        /// </summary>
        public Country(int id, string name)
        {
            Id = id;
            Name = name ?? string.Empty;
        }

        /// <summary>
        /// Возвращает строковое представление объекта.
        /// </summary>
        public override string ToString()
        {
            return $"{Id}: {Name}";
        }
    }
}
