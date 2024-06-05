using System;
using System.ComponentModel.DataAnnotations;

namespace NewWorldEmploymentServices.EntityModels
{
	public class User
	{
		[Key]
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool isdeleted { get; set; }
    }
}

