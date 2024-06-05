using System;
using NewWorldEmploymentServices.EntityModels;

namespace NewWorldEmploymentServices.Models
{
	public class JobNatureViewModel
	{
        public int JobNatureId { get; set; }
        public string? Name { get; set; }

        // Navigation Property
        public PostJob? Jobs { get; set; }
    }
}

