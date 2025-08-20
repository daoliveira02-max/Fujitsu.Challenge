using Fujitsu.Challenge.API.Interfaces;
using Fujitsu.Challenge.API.Models;
using Fujitsu.Challenge.API.Models.Books;
using Fujitsu.Challenge.API.Repositories;

namespace Fujitsu.Challenge.API.Services
{
    public class BookService : IBookService
    {
        private readonly IFileRepository<Book> _fileRepository;

        public BookService()
        {
            _fileRepository = new FileRepository<Book>("books.json");
        }

        public IResponse<List<Book>> Get()
        {
            var books = _fileRepository.GetAll();
            return Response<List<Book>>.Success(_fileRepository.GetAll());
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
                Year = request.Year
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
