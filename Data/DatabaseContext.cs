using lab5.Data.Models;

namespace lab5.Data
{
    public class DatabaseContext
    {
        public List<Country> Countries { get; set; } = new List<Country>();
        public List<Club> Clubs { get; set; } = new List<Club>();
        public List<Achievement> Achievements { get; set; } = new List<Achievement>();
    }
}