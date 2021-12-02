using System;
using System.ComponentModel.DataAnnotations;

namespace PKM.Model
{
    public class User
    {
        [Key]
        public Guid Guid { get; set; }

        public string FullName { get; set; }
    }
}