using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Author
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime Birthday { get; set; }
        public List<Book> Books { get; set; }

        public Author()
        { }

        public Author(Guid id, string firstName, string lastName, string email, DateTime birthday)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Birthday = birthday;
        }
    }
}
