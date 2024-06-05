using System;
using System.ComponentModel.DataAnnotations;

namespace NewWorldEmploymentServices.Models
{
	public class OrganizationRegistrationViewModel
	{
        [Required(ErrorMessage = "Please Enter Name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

