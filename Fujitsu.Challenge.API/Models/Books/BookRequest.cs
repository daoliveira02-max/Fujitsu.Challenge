namespace Fujitsu.Challenge.API.Models.Books
{
    public class BookRequest
    {
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string Publisher { get; set; }
        public int Year { get; set; }
    }
}
