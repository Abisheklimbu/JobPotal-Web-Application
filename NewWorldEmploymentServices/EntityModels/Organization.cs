using System;
using System.ComponentModel.DataAnnotations;

namespace NewWorldEmploymentServices.EntityModels
{
	public class Organization
	{
		[Key]
        public int OrganizationId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool isdeleted { get; set; }


        // Navigation Property
        public ICollection<PostJob> PostedJobs { get; set; }

    }
}

