using Fujitsu.Challenge.API.Interfaces;
using Fujitsu.Challenge.API.Models;
using Fujitsu.Challenge.API.Models.Books;
using Fujitsu.Challenge.API.Models.Settings;
using Fujitsu.Challenge.API.Repositories;
using Microsoft.Extensions.Options;

namespace Fujitsu.Challenge.API.Services
{
    public class BookService : IBookService
    {
        private readonly IFileRepository<Book> _fileRepository;

        public BookService(IOptions<StorageSettings> storageSettings)
        {
            _fileRepository = new FileRepository<Book>(storageSettings.Value.BooksFile, storageSettings.Value.Folder);
        }

        public IResponse<List<Book>> Get(int userId)
        {
            var books = _fileRepository.GetAll()
                .Where(b => b.UserId == userId)
                .ToList();

            if (!books.Any())
                return Response<List<Book>>.Failure("Books not found");

            return Response<List<Book>>.Success(books);
        }

        public IResponse<Book> Create(BookRequest request)
        {
            var books = _fileRepository.GetAll();

            var newBook = new Book
            {
                Id = books.Any() ? books.Max(b => b.Id) + 1 : 1,
                Title = request.Title,
                Author = request.Author,
                Publisher = request.Publisher,
                Year = request.Year,
                UserId = request.UserId
            };

            books.Add(newBook);
            _fileRepository.SaveAll(books);

            return Response<Book>.Success(newBook);
        }

        public IResponse<Book> Update(int id, BookRequest request)
        {
            var books = _fileRepository.GetAll();
            var book = books.FirstOrDefault(b => b.Id == id);
           
            if (book == null)
                return Response<Book>.Failure("Book not found");

            book.Title = request.Title;
            book.Author = request.Author;
            book.Publisher = request.Publisher;
            book.Year = request.Year;
            book.UserId = request.UserId;

            _fileRepository.SaveAll(books);

            return Response<Book>.Success(book);
        }

        public IResponse<int> Delete(int id)
        {
            var books = _fileRepository.GetAll();
            var book = books.FirstOrDefault(b => b.Id == id);

            if (book == null)
                return Response<int>.Failure("Book not found");

            books.Remove(book);
            _fileRepository.SaveAll(books);

            return Response<int>.Success(book.Id);
        }
    }
}
