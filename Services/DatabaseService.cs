using lab5.Data;
using lab5.Data.Models;
using System;
using System.Linq;

namespace lab5.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly DatabaseContext _context;
        
        public DatabaseService(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public void AddCountry(Country country)
        {
            if (CountryExists(country.Id))
                throw new ArgumentException($"Страна с ID {country.Id} уже существует");
            
            _context.Countries.Add(country);
            Console.WriteLine($"Добавлена страна: {country}");
        }
        
        public void AddClub(Club club)
        {
            if (ClubExists(club.Id))
                throw new ArgumentException($"Клуб с ID {club.Id} уже существует");
            
            if (!CountryExists(club.CountryId))
                throw new ArgumentException($"Страна с ID {club.CountryId} не существует");
            
            _context.Clubs.Add(club);
            Console.WriteLine($"Добавлен клуб: {club}");
        }
        
        public void AddAchievement(Achievement achievement)
        {
            if (AchievementExists(achievement.Id))
                throw new ArgumentException($"Достижение с ID {achievement.Id} уже существует");
            
            if (!ClubExists(achievement.ClubId))
                throw new ArgumentException($"Клуб с ID {achievement.ClubId} не существует");
            
            _context.Achievements.Add(achievement);
            Console.WriteLine($"Добавлено достижение для клуба {achievement.ClubId}");
        }
        
        public void RemoveCountry(int id)
        {
            var country = GetCountryById(id);
            if (country == null)
                throw new ArgumentException($"Страна с ID {id} не найдена");
            
            _context.Countries.Remove(country);
            Console.WriteLine($"Удалена страна: {country.Name}");
        }
        
        public void RemoveClub(int id)
        {
            var club = GetClubById(id);
            if (club == null)
                throw new ArgumentException($"Клуб с ID {id} не найден");
            
            _context.Clubs.Remove(club);
            Console.WriteLine($"Удален клуб: {club.Name}");
        }
        
        public void RemoveAchievement(int id)
        {
            var achievement = GetAchievementById(id);
            if (achievement == null)
                throw new ArgumentException($"Достижение с ID {id} не найдено");
            
            _context.Achievements.Remove(achievement);
            Console.WriteLine($"Удалено достижение ID {id}");
        }
        
        public void ViewAll()
        {
            Console.WriteLine("\n=== СТРАНЫ ===");
            foreach (var country in _context.Countries.Take(10))
                Console.WriteLine(country);
            if (_context.Countries.Count > 10)
                Console.WriteLine($"... и ещё {_context.Countries.Count - 10} стран");
            
            Console.WriteLine("\n=== КЛУБЫ ===");
            foreach (var club in _context.Clubs.Take(10))
                Console.WriteLine(club);
            if (_context.Clubs.Count > 10)
                Console.WriteLine($"... и ещё {_context.Clubs.Count - 10} клубов");
            
            Console.WriteLine("\n=== ДОСТИЖЕНИЯ ===");
            foreach (var achievement in _context.Achievements.Take(10))
                Console.WriteLine(achievement);
            if (_context.Achievements.Count > 10)
                Console.WriteLine($"... и ещё {_context.Achievements.Count - 10} достижений");
        }
        
        public Country GetCountryById(int id) => _context.Countries.FirstOrDefault(c => c.Id == id);
        public Club GetClubById(int id) => _context.Clubs.FirstOrDefault(c => c.Id == id);
        public Achievement GetAchievementById(int id) => _context.Achievements.FirstOrDefault(a => a.Id == id);
        public bool CountryExists(int id) => _context.Countries.Any(c => c.Id == id);
        public bool ClubExists(int id) => _context.Clubs.Any(c => c.Id == id);
        public bool AchievementExists(int id) => _context.Achievements.Any(a => a.Id == id);
    }
}