using System;
using NewWorldEmploymentServices.EntityModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewWorldEmploymentServices.Models
{
	public class PostJobViewModel
	{
        public int JobId { get; set; }

        public string? JobName { get; set; }

        public int? OrganizationId { get; set; }

        public string? OrgImage { get; set; }

        public string? JobLocation { get; set; }

        public decimal? Salary { get; set; }

        public int? Vacancy { get; set; }

        public int JobCategoryId { get; set; }

        public int JobNatureId { get; set; }

        public string? Education { get; set; }

        public string? WorkExperience { get; set; }

        public string? Skills { get; set; }

        public DateTime? PostedDate { get; set; }

        public DateTime? ExpiredDate { get; set; }

        public string? ContactNo { get; set; }

        public string? Email { get; set; }

        public string? Description { get; set; }

        public bool isdeleted { get; set; }

        public List<PostJobViewModel> PostedJobList { get; set; }

        // Navigation Properties
        public Organization Organization { get; set; }
        public JobCategory Category { get; set; }
        public JobNature Nature { get; set; }
    }

}

