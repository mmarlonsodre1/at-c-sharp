using System;
using System.Collections.Generic;
using Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private AuthorRepository AuthorRepository { get; set; }
        private BookRepository BookRepository { get; set; }

        public AuthorsController(AuthorRepository authorRepository, BookRepository bookRepository)
        {
            AuthorRepository = authorRepository;
            BookRepository = bookRepository;
        }

        [HttpGet]
        [Authorize]
        public List<Author> Get()
        {
            return AuthorRepository.GetAuthors();
        }

        [HttpGet("{id}")]
        [Authorize]
        public Author Get(string id)
        {
            return AuthorRepository.GetAuthorById(id);
        }

        [HttpGet]
        [Authorize]
        [Route("Books/{id}")]
        public List<Book> GetBooks([FromRoute] string id)
        {
            return BookRepository.GetBookByAuthorId(id);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Author model)
        {
            model.Id = Guid.NewGuid();
            AuthorRepository.Save(model);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] Author model)
        {
            AuthorRepository.Update(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(Guid id)
        {
            AuthorRepository.Delete(id);
            return Ok();
        }
    }
}
