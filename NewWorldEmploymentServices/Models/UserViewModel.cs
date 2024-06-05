using System;
using NewWorldEmploymentServices.EntityModels;

namespace NewWorldEmploymentServices.Models
{
	public class UserViewModel
	{
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool isdeleted { get; set; }

        public List<User> Users { get; set; }
        public List<UserViewModel> UsersList { get; set; }
    }
}

