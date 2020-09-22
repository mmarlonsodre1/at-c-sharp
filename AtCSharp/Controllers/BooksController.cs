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
    public class BooksController : ControllerBase
    {
        private BookRepository BookRepository { get; set; }

        public BooksController(BookRepository bookRepository)
        {
            BookRepository = bookRepository;
        }

        [HttpGet]
        [Authorize]
        public List<Book> Get()
        {
            return BookRepository.GetBooks();
        }

        [HttpGet("{id}")]
        [Authorize]
        public Book Get(string id)
        {
            return BookRepository.GetBookById(id);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Book model)
        {
            model.Id = Guid.NewGuid();
            BookRepository.Save(model);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] Book model)
        {
            BookRepository.Update(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(Guid id)
        {
            BookRepository.Delete(id);
            return Ok();
        }
    }
}
