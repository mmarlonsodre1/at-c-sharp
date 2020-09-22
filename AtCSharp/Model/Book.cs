using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Book
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.PhoneNumber)]
        public int Year { get; set; }
        public List<Author> Authors { get; set; }

        public Book()
        { }

        public Book(Guid id, string title, string isbn, int year)
        {
            Id = id;
            Title = title;
            ISBN = isbn;
            Year = year;
        }

        public Book(Guid id, string title, string isbn, int year, List<Author> authors)
        {
            Id = id;
            Title = title;
            ISBN = isbn;
            Year = year;
            Authors = authors;
        }
    }
}
