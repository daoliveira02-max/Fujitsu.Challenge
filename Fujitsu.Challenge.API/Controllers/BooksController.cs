using Fujitsu.Challenge.API.Interfaces;
using Fujitsu.Challenge.API.Models.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fujitsu.Challenge.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _booksService;

        public BooksController(IBookService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var userId = int.Parse(User.FindFirst("id")!.Value);

            var result = _booksService.Get(userId);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody] BookRequest request)
        {
            request.UserId = int.Parse(User.FindFirst("id")!.Value);

            var result = _booksService.Create(request);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] BookRequest request)
        {
            request.UserId = int.Parse(User.FindFirst("id")!.Value);

            var result = _booksService.Update(id, request);

            if (!result.IsSuccess) 
                return NotFound(result.FailureReason);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _booksService.Delete(id);

            if (!result.IsSuccess)
                return NotFound(result.FailureReason);

            return Ok(result);
        }
    }
}
