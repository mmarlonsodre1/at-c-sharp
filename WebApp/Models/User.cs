using System;
using System.ComponentModel.DataAnnotations;

namespace FundamentosCsharpTp3.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Username { get; set; }

        public User(Guid id, string userName)
        {
            Id = id;
            Username = userName;
        }

        public User()
        {}
    }
}
