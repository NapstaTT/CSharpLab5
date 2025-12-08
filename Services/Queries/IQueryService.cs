using System.Collections.Generic;
using lab5.Data.Models;

namespace lab5.Services.Queries
{
    public interface IQueryService
    {
        List<Country> GetCountriesSortedByName();
        
        List<dynamic> GetClubsWithCountryNames();
        
        string GetCountryWithMostGoldMedals();
        
        List<string> GetClubsWithGoldMedalsButNoCups();
        
        int GetCountryIdOfChampionWithoutCups();
    }
}