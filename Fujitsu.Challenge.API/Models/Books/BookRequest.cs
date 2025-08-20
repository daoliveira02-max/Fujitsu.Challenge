namespace Fujitsu.Challenge.API.Models.Books
{
    public class BookRequest
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }
        public int UserId { get; set; }
    }
}
