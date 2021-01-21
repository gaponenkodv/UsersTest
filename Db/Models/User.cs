using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppTest.Db.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, StringLength(150, MinimumLength = 3)]
        public string Login { get; set; }
        [Required, StringLength(150, MinimumLength = 3)]
        public string Name { get; set; }
        [Required, EmailAddress, StringLength(150, MinimumLength = 3)]
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}
