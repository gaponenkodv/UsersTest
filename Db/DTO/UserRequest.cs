using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppTest.Db.DTO
{
    public class UserAuthRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class UserRequest
    {
        public int Id { get; set; }
        public string Login { get; set; }
        [Required, StringLength(150, MinimumLength = 3)]
        public string Name { get; set; }
        [Required, EmailAddress, StringLength(150, MinimumLength = 3)]
        public string Email { get; set; }
    }
}
