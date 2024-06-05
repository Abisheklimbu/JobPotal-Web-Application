using System;
using System.ComponentModel.DataAnnotations;

namespace NewWorldEmploymentServices.EntityModels
{
	public class JobNature
	{
        [Key]
        public int JobNatureId { get; set; }
        public string? Name { get; set; }

        // Navigation Property
        public ICollection<PostJob> Jobs { get; set; }
    }
}

