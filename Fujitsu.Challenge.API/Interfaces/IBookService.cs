using Fujitsu.Challenge.API.Models;
using Fujitsu.Challenge.API.Models.Books;

namespace Fujitsu.Challenge.API.Interfaces
{
    public interface IBookService
    {
        IResponse<List<Book>> Get(int userId);
        IResponse<Book> Create(BookRequest request);
        IResponse<Book> Update(int id, BookRequest request);
        IResponse<int> Delete(int id);
    }
}
