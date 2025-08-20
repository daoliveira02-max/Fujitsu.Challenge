namespace Fujitsu.Challenge.API.Models
{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string Publisher { get; set; }
        public int Year { get; set; }
        public int UserId { get; set; }
    }
}
