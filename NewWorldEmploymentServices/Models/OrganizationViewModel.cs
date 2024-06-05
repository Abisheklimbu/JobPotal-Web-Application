using System;
using NewWorldEmploymentServices.EntityModels;

namespace NewWorldEmploymentServices.Models
{
	public class OrganizationViewModel
	{
        public int OrganizationId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool isdeleted { get; set; }

        public List<Organization> Organizations { get; set; }
        public List<OrganizationViewModel> OrganizationsList { get; set; }
        // Navigation Property
        public ICollection<PostJob> PostedJobs { get; set; }

       
    }
}

