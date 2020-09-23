using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int Year { get; set; }
        public Guid AuthorId { get; set; }

        public Book()
        { }

        public Book(Guid id, string title, string isbn, int year)
        {
            Id = id;
            Title = title;
            ISBN = isbn;
            Year = year;
        }

        public Book(Guid id, string title, string isbn, int year, Guid authorId)
        {
            Id = id;
            Title = title;
            ISBN = isbn;
            Year = year;
            AuthorId = authorId;
        }
    }
}
