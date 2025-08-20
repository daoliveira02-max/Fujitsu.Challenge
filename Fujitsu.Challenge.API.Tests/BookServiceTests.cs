using Fujitsu.Challenge.API.Interfaces;
using Fujitsu.Challenge.API.Models.Books;
using Fujitsu.Challenge.API.Models.Settings;
using Fujitsu.Challenge.API.Services;
using Microsoft.Extensions.Options;

namespace Fujitsu.Challenge.API.Tests
{
    [TestClass]
    public class BookServiceTests
    {
        private BookService _bookService;
        
        [TestInitialize]
        public void Setup()
        {
            var jwtSettings = Options.Create(new JwtSettings
            {
                Key = "djasçldfjsalkfnaslkdasdsadsavnad",
                Issuer = "Test",
                Audience = "TestAudience",
                ExpireMinutes = 5
            });

            var storageSettings = Options.Create(new StorageSettings
            {
                BooksFile = "books_test.json",
                Folder = "Data"
            });

            var filePath = Path.Combine("Data", "books_test.json");
            if (File.Exists(filePath))
                File.WriteAllText(filePath, "[]");

            _bookService = new BookService(storageSettings);
        }

        [TestMethod]
        public void Create_Should_Add_New_Book()
        {
            var result = _bookService.Create(new BookRequest
            {
                Title = "Teste",
                Author = "Roberto",
                Publisher = "Teste",
                Year = 2008,
                UserId = 1
            });

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Teste", result.Content.Title);
        }

        [TestMethod]
        public void Get_Should_Return_Books_For_User()
        {
            _bookService.Create(new BookRequest
            {
                Title = "Teste",
                Author = "Roberto",
                Publisher = "Teste",
                Year = 2008,
                UserId = 1
            });

            var result = _bookService.Get(1);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(1, result.Content.Count);
            Assert.AreEqual("Teste", result.Content[0].Title);
        }

        [TestMethod]
        public void Update_Should_Change_Book_Title()
        {
            var created = _bookService.Create(new BookRequest
            {
                Title = "Teste",
                Author = "Roberto",
                Publisher = "Teste",
                Year = 2008,
                UserId = 1
            });

            var updated = _bookService.Update(created.Content.Id, new BookRequest
            {
                Title = "Teste1",
                Author = "Roberto",
                Publisher = "Teste",
                Year = 2008,
                UserId = 1
            });

            Assert.IsTrue(updated.IsSuccess);
            Assert.AreEqual("Teste1", updated.Content.Title);
        }
    }
}
