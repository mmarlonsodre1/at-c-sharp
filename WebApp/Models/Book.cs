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

        [Required(ErrorMessage = "Campo obrigatório")]
        public string AuthorId { get; set; }
    }
}
