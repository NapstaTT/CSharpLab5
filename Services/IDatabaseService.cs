using lab5.Data.Models;

namespace lab5.Services
{
    public interface IDatabaseService
    {
        void AddCountry(Country country);
        void AddClub(Club club);
        void AddAchievement(Achievement achievement);
        void RemoveCountry(int id);
        void RemoveClub(int id);
        void RemoveAchievement(int id);
        void ViewAll();
        Country GetCountryById(int id);
        Club GetClubById(int id);
        Achievement GetAchievementById(int id);
        bool CountryExists(int id);
        bool ClubExists(int id);
        bool AchievementExists(int id);
    }
}