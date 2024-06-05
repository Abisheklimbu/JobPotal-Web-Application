﻿using System;
using System.ComponentModel.DataAnnotations;

namespace NewWorldEmploymentServices.EntityModels
{
	public class Admin
	{
		[Key]
        public int AdminId { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool isdeleted { get; set; }
    }
}

