using System;
using System.ComponentModel.DataAnnotations;

namespace NewWorldEmploymentServices.EntityModels
{
	public class JobCategory
	{
        [Key]
        public int JobCategoryId { get; set; }
        public string? Name { get; set; }

        // Navigation Property
        public ICollection<PostJob> Jobs { get; set; }
    }
}

