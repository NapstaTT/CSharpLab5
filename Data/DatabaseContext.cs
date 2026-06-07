///FIXME:
/// - Добавлена XML-документация
/// - Приведены скобки к стилю Allman
/// - Удалены лишние пустые строки
/// - Свойства приведены к IReadOnlyCollection, но оставлены List для совместимости

using System.Collections.Generic;
using lab5.Data.Models;

namespace lab5.Data
{
    /// <summary>
    /// Хранит коллекции данных, загруженных из Excel.
    /// </summary>
    public class DatabaseContext
    {
        /// <summary>
        /// Список стран.
        /// </summary>
        public List<Country> Countries { get; }

        /// <summary>
        /// Список клубов.
        /// </summary>
        public List<Club> Clubs { get; }

        /// <summary>
        /// Список достижений.
        /// </summary>
        public List<Achievement> Achievements { get; }

        /// <summary>
        /// Создаёт новый пустой контекст базы данных.
        /// </summary>
        public DatabaseContext()
        {
            Countries = new List<Country>();
            Clubs = new List<Club>();
            Achievements = new List<Achievement>();
        }
    }
}
