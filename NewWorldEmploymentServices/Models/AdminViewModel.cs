using System;
using System.ComponentModel.DataAnnotations;
using NewWorldEmploymentServices.EntityModels;

namespace NewWorldEmploymentServices.Models
{
	public class AdminViewModel
	{
        public int AdminId { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool isdeleted { get; set; }

        public List<Admin> Admins { get; set; }
    }
}

