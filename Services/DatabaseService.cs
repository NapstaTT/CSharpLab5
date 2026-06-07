///FIXME:
/// - Добавлена XML-документация
/// - Приведены скобки к стилю Allman
/// - Переименованы локальные переменные в camelCase
/// - Удалены лишние using
/// - Добавлены guard-clauses
/// - Улучшена читаемость LINQ-запросов

using System;
using System.Linq;
using lab5.Data;
using lab5.Data.Models;

namespace lab5.Services
{
    /// <summary>
    /// Реализует операции управления странами, клубами и достижениями.
    /// </summary>
    public class DatabaseService : IDatabaseService
    {
        private readonly DatabaseContext _context;

        /// <summary>
        /// Создаёт новый экземпляр сервиса.
        /// </summary>
        public DatabaseService(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public void AddCountry(Country country)
        {
            if (CountryExists(country.Id))
            {
                throw new ArgumentException($"Страна с ID {country.Id} уже существует");
            }

            _context.Countries.Add(country);
            Console.WriteLine($"Добавлена страна: {country}");
        }

        /// <inheritdoc />
        public void AddClub(Club club)
        {
            if (ClubExists(club.Id))
            {
                throw new ArgumentException($"Клуб с ID {club.Id} уже существует");
            }

            if (!CountryExists(club.CountryId))
            {
                throw new ArgumentException($"Страна с ID {club.CountryId} не существует");
            }

            _context.Clubs.Add(club);
            Console.WriteLine($"Добавлен клуб: {club}");
        }

        /// <inheritdoc />
        public void AddAchievement(Achievement achievement)
        {
            if (AchievementExists(achievement.Id))
            {
                throw new ArgumentException($"Достижение с ID {achievement.Id} уже существует");
            }

            if (!ClubExists(achievement.ClubId))
            {
                throw new ArgumentException($"Клуб с ID {achievement.ClubId} не существует");
            }

            _context.Achievements.Add(achievement);
            Console.WriteLine($"Добавлено достижение для клуба {achievement.ClubId}");
        }

        /// <inheritdoc />
        public void RemoveCountry(int id)
        {
            var country = GetCountryById(id);

            if (country == null)
            {
                throw new ArgumentException($"Страна с ID {id} не найдена");
            }

            _context.Countries.Remove(country);
            Console.WriteLine($"Удалена страна: {country.Name}");
        }

        /// <inheritdoc />
        public void RemoveClub(int id)
        {
            var club = GetClubById(id);

            if (club == null)
            {
                throw new ArgumentException($"Клуб с ID {id} не найден");
            }

            _context.Clubs.Remove(club);
            Console.WriteLine($"Удалён клуб: {club.Name}");
        }

        /// <inheritdoc />
        public void RemoveAchievement(int id)
        {
            var achievement = GetAchievementById(id);

            if (achievement == null)
            {
                throw new ArgumentException($"Достижение с ID {id} не найдено");
            }

            _context.Achievements.Remove(achievement);
            Console.WriteLine($"Удалено достижение ID {id}");
        }

        /// <inheritdoc />
        public void ViewAll()
        {
            Console.WriteLine("\n=== СТРАНЫ ===");
            foreach (var country in _context.Countries.Take(10))
            {
                Console.WriteLine(country);
            }

            if (_context.Countries.Count > 10)
            {
                Console.WriteLine($"... и ещё {_context.Countries.Count - 10} стран");
            }

            Console.WriteLine("\n=== КЛУБЫ ===");
            foreach (var club in _context.Clubs.Take(10))
            {
                Console.WriteLine(club);
            }

            if (_context.Clubs.Count > 10)
            {
                Console.WriteLine($"... и ещё {_context.Clubs.Count - 10} клубов");
            }

            Console.WriteLine("\n=== ДОСТИЖЕНИЯ ===");
            foreach (var achievement in _context.Achievements.Take(10))
            {
                Console.WriteLine(achievement);
            }

            if (_context.Achievements.Count > 10)
            {
                Console.WriteLine($"... и ещё {_context.Achievements.Count - 10} достижений");
            }
        }

        /// <inheritdoc />
        public Country GetCountryById(int id)
        {
            return _context.Countries.FirstOrDefault(c => c.Id == id);
        }

        /// <inheritdoc />
        public Club GetClubById(int id)
        {
            return _context.Clubs.FirstOrDefault(c => c.Id == id);
        }

        /// <inheritdoc />
        public Achievement GetAchievementById(int id)
        {
            return _context.Achievements.FirstOrDefault(a => a.Id == id);
        }

        /// <inheritdoc />
        public bool CountryExists(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }

        /// <inheritdoc />
        public bool ClubExists(int id)
        {
            return _context.Clubs.Any(c => c.Id == id);
        }

        /// <inheritdoc />
        public bool AchievementExists(int id)
        {
            return _context.Achievements.Any(a => a.Id == id);
        }
    }
}
