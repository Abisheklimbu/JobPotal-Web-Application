using System;
using NewWorldEmploymentServices.EntityModels;
using System.ComponentModel.DataAnnotations;

namespace NewWorldEmploymentServices.Models
{
	public class JobCategoryViewModel
	{
        public int JobCategoryId { get; set; }
        public string? Name { get; set; }

        // Navigation Property
        public ICollection<PostJob> Jobs { get; set; }
    }
}

