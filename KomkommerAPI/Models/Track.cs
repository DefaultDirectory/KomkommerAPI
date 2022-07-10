namespace KomkommerAPI.Models
{
    public class Track
    {
        public Guid Id { get; set; }
        public string? Artist { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
    }
}
