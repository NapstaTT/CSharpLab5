namespace lab5.Data.Models
{
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        
        public override string ToString() => $"{Id}: {Name} (Country: {CountryId})";
    }
}