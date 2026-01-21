namespace lab5.Data.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public Country() { }
        
        public Country(int id, string name)
        {
            Id = id;
            Name = name ?? string.Empty;
        }
        
        public override string ToString() => $"{Id}: {Name}";
    }
}