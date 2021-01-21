using System;
using System.Collections.Generic;
using AppTest.Db.Models;

namespace AppTest.Db.DTO
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<RoleResponse> Roles { get; set; }
    }
}
