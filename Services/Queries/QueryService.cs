///FIXME:
/// - Добавлена XML-документация
/// - Приведены скобки к стилю Allman
/// - Улучшены LINQ-запросы
/// - Переименованы локальные переменные
/// - Удалены лишние using

using System;
using System.Collections.Generic;
using System.Linq;
using lab5.Data;
using lab5.Data.Models;

namespace lab5.Services.Queries
{
    /// <summary>
    /// Реализует набор LINQ-запросов к данным.
    /// </summary>
    public class QueryService : IQueryService
    {
        private readonly DatabaseContext _context;

        /// <summary>
        /// Создаёт новый экземпляр сервиса запросов.
        /// </summary>
        public QueryService(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public List<Country> GetCountriesSortedByName()
        {
            return _context.Countries
                .OrderBy(c => c.Name)
                .ToList();
        }

        /// <inheritdoc />
        public List<dynamic> GetClubsWithCountryNames()
        {
            var query =
                from club in _context.Clubs
                join country in _context.Countries on club.CountryId equals country.Id
                orderby country.Name, club.Name
                select new
                {
                    ClubId = club.Id,
                    ClubName = club.Name,
                    CountryName = country.Name
                };

            return query.ToList<dynamic>();
        }

        /// <inheritdoc />
        public string GetCountryWithMostGoldMedals()
        {
            var result =
                (from achievement in _context.Achievements
                 join club in _context.Clubs on achievement.ClubId equals club.Id
                 join country in _context.Countries on club.CountryId equals country.Id
                 group achievement by country.Name
                 into g
                 select new
                 {
                     CountryName = g.Key,
                     TotalGold = g.Sum(a => a.G)
                 })
                .OrderByDescending(x => x.TotalGold)
                .FirstOrDefault();

            return result != null
                ? $"{result.CountryName}: {result.TotalGold} золотых медалей"
                : "Нет данных";
        }

        /// <inheritdoc />
        public List<string> GetClubsWithGoldMedalsButNoCups()
        {
            var query =
                from achievement in _context.Achievements
                where achievement.G > 0 && achievement.C == 0
                join club in _context.Clubs on achievement.ClubId equals club.Id
                join country in _context.Countries on club.CountryId equals country.Id
                orderby achievement.G descending
                select $"{club.Name} ({country.Name}): {achievement.G} золотых медалей, 0 кубков";

            return query.Distinct().ToList();
        }

        /// <inheritdoc />
        public int GetCountryIdOfChampionWithoutCups()
        {
            var clubsWithoutCups =
                from achievement in _context.Achievements
                where achievement.G > 0 && achievement.C == 0
                join club in _context.Clubs on achievement.ClubId equals club.Id
                group new { achievement, club } by club.Id
                into g
                select new
                {
                    ClubId = g.Key,
                    Club = g.First().club,
                    GoldMedals = g.Sum(x => x.achievement.G)
                };

            if (!clubsWithoutCups.Any())
            {
                return -1;
            }

            var topClub = clubsWithoutCups
                .OrderByDescending(x => x.GoldMedals)
                .First();

            var maxGold = topClub.GoldMedals;

            var clubsWithMaxGold = clubsWithoutCups
                .Where(x => x.GoldMedals == maxGold)
                .ToList();

            if (clubsWithMaxGold.Count > 1)
            {
                return clubsWithMaxGold
                    .Select(c => c.Club.CountryId)
                    .Max();
            }

            return topClub.Club.CountryId;
        }
    }
}
