namespace lab5.Data.Models
{
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        
        public Club() { }

        public Club(int id, string name, int countryId)
        {
            Id = id;
            Name = name ?? string.Empty;
            CountryId = countryId;
        }
        
        public override string ToString() => $"{Id}: {Name} (Country: {CountryId})";
    }
}