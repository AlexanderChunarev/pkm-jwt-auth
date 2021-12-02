using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PKM.Model
{
    [Index(nameof(Login))]
    public class UserCredentials
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}