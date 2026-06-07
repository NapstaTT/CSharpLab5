///FIXME:
/// - Добавлена XML-документация
/// - Приведены скобки к стилю Allman
/// - Упорядочены методы по смыслу

using System.Collections.Generic;
using lab5.Data.Models;

namespace lab5.Services.Queries
{
    /// <summary>
    /// Определяет набор LINQ-запросов к данным.
    /// </summary>
    public interface IQueryService
    {
        /// <summary>
        /// Возвращает список стран, отсортированных по имени.
        /// </summary>
        List<Country> GetCountriesSortedByName();

        /// <summary>
        /// Возвращает клубы вместе с названиями стран.
        /// </summary>
        List<dynamic> GetClubsWithCountryNames();

        /// <summary>
        /// Возвращает страну с наибольшим количеством золотых медалей.
        /// </summary>
        string GetCountryWithMostGoldMedals();

        /// <summary>
        /// Возвращает клубы, имеющие золотые медали, но не имеющие кубков.
        /// </summary>
        List<string> GetClubsWithGoldMedalsButNoCups();

        /// <summary>
        /// Возвращает ID страны клуба-чемпиона без кубков.
        /// </summary>
        int GetCountryIdOfChampionWithoutCups();
    }
}
