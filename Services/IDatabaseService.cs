///FIXME:
/// - Добавлена XML-документация ко всем методам
/// - Приведены скобки к стилю Allman
/// - Удалены лишние пустые строки
/// - Интерфейс структурирован по логическим блокам

using lab5.Data.Models;

namespace lab5.Services
{
    /// <summary>
    /// Определяет операции управления странами, клубами и достижениями.
    /// </summary>
    public interface IDatabaseService
    {
        /// <summary>
        /// Добавляет новую страну.
        /// </summary>
        void AddCountry(Country country);

        /// <summary>
        /// Добавляет новый клуб.
        /// </summary>
        void AddClub(Club club);

        /// <summary>
        /// Добавляет новое достижение.
        /// </summary>
        void AddAchievement(Achievement achievement);

        /// <summary>
        /// Удаляет страну по ID.
        /// </summary>
        void RemoveCountry(int id);

        /// <summary>
        /// Удаляет клуб по ID.
        /// </summary>
        void RemoveClub(int id);

        /// <summary>
        /// Удаляет достижение по ID.
        /// </summary>
        void RemoveAchievement(int id);

        /// <summary>
        /// Выводит первые элементы всех таблиц.
        /// </summary>
        void ViewAll();

        /// <summary>
        /// Возвращает страну по ID.
        /// </summary>
        Country GetCountryById(int id);

        /// <summary>
        /// Возвращает клуб по ID.
        /// </summary>
        Club GetClubById(int id);

        /// <summary>
        /// Возвращает достижение по ID.
        /// </summary>
        Achievement GetAchievementById(int id);

        /// <summary>
        /// Проверяет существование страны.
        /// </summary>
        bool CountryExists(int id);

        /// <summary>
        /// Проверяет существование клуба.
        /// </summary>
        bool ClubExists(int id);

        /// <summary>
        /// Проверяет существование достижения.
        /// </summary>
        bool AchievementExists(int id);
    }
}
