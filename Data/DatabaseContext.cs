using lab5.Data.Models;

namespace lab5.Data
{
    public class DatabaseContext
    {
        public List<Country> Countries { get; private set; }
        public List<Club> Clubs { get; private set; }
        public List<Achievement> Achievements { get; private set; }
        
        public DatabaseContext()
        {
            Countries = new List<Country>();
            Clubs = new List<Club>();
            Achievements = new List<Achievement>();
        }
    }
}