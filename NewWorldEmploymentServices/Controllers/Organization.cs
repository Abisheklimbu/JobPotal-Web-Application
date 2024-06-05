using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims; // Added for Claims
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewWorldEmploymentServices.AppWebDbContext;
using NewWorldEmploymentServices.EntityModels;
using NewWorldEmploymentServices.Models;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NewWorldEmploymentServices.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly AppDbContext _dbContext;
        public OrganizationController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OrganizationsProfile()
        {
            return View();
        }
        public IActionResult PostsJob()
        {
            var viewModel = new PostJobViewModel();

            // Retrieve the organization ID from the session
            var organizationId = HttpContext.Session.GetInt32("OrganizationId");
            if (organizationId != null)
            {
                // Find the organization by ID
                var organization = _dbContext.Organizations.FirstOrDefault(o =>
                o.OrganizationId == organizationId);
                if (organization != null)
                {
                    // Set the organization name in the view model
                    viewModel.OrganizationId = organization.OrganizationId;
                }
            }

            // Your existing code for populating dropdown lists
            var categories = _dbContext.JobCategories.ToList();
            ViewBag.Categories = new SelectList(categories, "JobCategoryId", "Name");

            var natures = _dbContext.JobNatures.ToList();
            ViewBag.Natures = new SelectList(natures, "JobNatureId", "Name");

            return View("~/Views/Organization/PostsJob.cshtml", viewModel);
        }

        [HttpPost]
        public IActionResult PostsJob(PostJobViewModel model, IFormFile? file)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Model state is not valid, return the view with the
                // model to display validation errors
                return View("~/Views/Organization/PostsJob.cshtml", model);
            }

            // Fetch the organization ID from the session
            var organizationId = HttpContext.Session.GetInt32("OrganizationId");

            // Check if the organization ID is null or not found
            if (organizationId == null)
            {
                // Handle the scenario where the organization ID is not found in the session
                ModelState.AddModelError(string.Empty, "Organization ID not found in the session.");
                return View("~/Views/Organization/PostsJob.cshtml", model);
            }

            // Check if the organization with the retrieved organization ID exists in the database
            var organization = _dbContext.Organizations.FirstOrDefault(o => o.OrganizationId == organizationId);

            if (organization == null)
            {
                // Handle the scenario where the organization corresponding to the retrieved ID is not found
                ModelState.AddModelError(string.Empty, "Organization not found in the database.");
                return View("~/Views/Organization/PostsJob.cshtml", model);
            }

            // Proceed with creating the job posting
            // Handle file upload and get image path

            string ImagePath = null;
            if (file != null)
            {
                // Handle file upload and get image path
                var GuidId = Guid.NewGuid().ToString();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string fileName = GuidId + file.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                ImagePath = "/Files/" + GuidId + file.FileName;
            }

            // Create a new PostJob entity and populate its properties
            PostJob entitypostjobs = new PostJob
            {
                JobName = model.JobName,
                OrganizationId = organizationId.Value, // Assign the retrieved organization ID
                OrgImage = ImagePath,
                JobLocation = model.JobLocation,
                Salary = model.Salary,
                JobCategoryId = model.JobCategoryId,
                JobNatureId = model.JobNatureId,
                WorkExperience = model.WorkExperience,
                PostedDate = model.PostedDate,
                ExpiredDate = model.ExpiredDate,
                Vacancy = model.Vacancy,
                Education = model.Education,
                Skills = model.Skills,
                ContactNo = model.ContactNo,
                Email = model.Email,
                Description = model.Description
            };

            // Add the new PostJob entity to the context and save changes
            _dbContext.PostJobs.Add(entitypostjobs);
            _dbContext.SaveChanges();

            // Redirect to the Index action method
            return RedirectToAction("Index");
        }


        public IActionResult ListsJobs()
        {
            // Retrieve the organization ID from the session
            var organizationId = HttpContext.Session.GetInt32("OrganizationId");

            if (organizationId != null)
            {
                // Retrieve jobs only for the logged-in organization
                var jobs = _dbContext.PostJobs
                    .Where(job => job.OrganizationId == organizationId)
                    .Where(job => job.OrganizationId == organizationId && !job.isdeleted)
                    .ToList();

                // Retrieve organization names
                var organizationNames = _dbContext.Organizations
                    .Where(org => org.OrganizationId == organizationId)
                    .ToDictionary(org => org.OrganizationId, org => org.Name);

                // Pass jobs and organization names to the view
                ViewBag.OrganizationNames = organizationNames;
                return View(jobs);
            }

            // If organization ID is not found in the session, handle accordingly
            // For example, redirect to login or show an error message
            return RedirectToAction("Login", "Account"); // Redirect to login page
        }


        public IActionResult EditJob(int id)
        {
            PostJobViewModel model = new PostJobViewModel();
            var data = _dbContext.PostJobs.FirstOrDefault(x => x.JobId == id);
            if (data != null)
            {
                model.OrganizationId = data.OrganizationId;
                model.JobId = data.JobId;
                model.JobName = data.JobName;
                model.JobLocation = data.JobLocation;
                model.JobCategoryId = (int)data.JobCategoryId;
                model.JobNatureId = (int)data.JobNatureId;
                model.WorkExperience = data.WorkExperience;
                model.Salary = data.Salary;
                model.PostedDate = data.PostedDate;
                model.ExpiredDate = data.ExpiredDate;
                model.Vacancy = data.Vacancy;
                model.Education = data.Education;
                model.Skills = data.Skills;
                model.ContactNo = data.ContactNo;
                model.Email = data.Email;
                model.Description = data.Description;

            }

            // You may want to populate ViewBag.Categories and ViewBag.Natures here
            // Populate ViewBag.Categories
            ViewBag.Categories = _dbContext.JobCategories
                                    .Select(c => new SelectListItem { Value = c.JobCategoryId.ToString(), Text = c.Name })
                                    .ToList();

            // Populate ViewBag.Natures
            ViewBag.Natures = _dbContext.JobNatures
                                    .Select(n => new SelectListItem { Value = n.JobNatureId.ToString(), Text = n.Name })
                                    .ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult EditJob(PostJobViewModel model, IFormFile file)
        {
            // Check if the model state is not valid
            if (ModelState.IsValid)
            {
                // If the model state is not valid, return the view with the model
                return View(model);
            }

            // Update the Job details in the database
            var existingJob = _dbContext.PostJobs.FirstOrDefault(x => x.JobId == model.JobId);
            if (existingJob != null)
            {
                string ImagePath = null;
                if (file != null)
                {
                    var GuidId = Guid.NewGuid().ToString();
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    string fileName = GuidId + file.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    ImagePath = "/Files/" + GuidId + file.FileName;
                }
                // Update the existing job entity
                existingJob.OrganizationId = model.OrganizationId;
                existingJob.JobName = model.JobName;
                existingJob.JobLocation = model.JobLocation;
                existingJob.JobCategoryId = model.JobCategoryId;
                existingJob.JobNatureId = model.JobNatureId;
                existingJob.WorkExperience = model.WorkExperience;
                existingJob.Salary = model.Salary;
                existingJob.PostedDate = model.PostedDate;
                existingJob.ExpiredDate = model.ExpiredDate;
                existingJob.Description = model.Description;
                existingJob.Vacancy = model.Vacancy;
                existingJob.Education = model.Education;
                existingJob.Skills = model.Skills;
                existingJob.ContactNo = model.ContactNo;
                existingJob.Email = model.Email;
                existingJob.OrgImage = ImagePath;
                _dbContext.SaveChanges();

                // Redirect to the view job details page after successful update
                return RedirectToAction("ListsJobs", "Organization");
            }
            else
            {
                // Handle the case where the job is not found
                // Return an appropriate response, such as a 404 Not Found
                return NotFound();
            }
        }


        public IActionResult DeleteJob(int id)
       {
            var data = _dbContext.PostJobs
                .Where(x => x.JobId == id).FirstOrDefault();
            if (data != null)
            {
                data.isdeleted = true;
                _dbContext.Entry(data).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            return RedirectToAction("ListsJobs");
        }

        public IActionResult DeletedList()
        {
            // Retrieve the organization ID from the session, assuming it's stored in HttpContext.Session
            var organizationId = HttpContext.Session.GetInt32("OrganizationId");

            if (organizationId != null)
            {
                // Retrieve deleted jobs for the organization ID
                var deletedJobs = _dbContext.PostJobs
                    .Where(job => job.isdeleted && job.OrganizationId == organizationId)
                    .Select(job => new PostJobViewModel
                    {
                        JobId = job.JobId,
                        JobName = job.JobName,
                        JobLocation = job.JobLocation
                        // Add other properties as needed
                    })
                    .ToList();

                // Populate the PostedJobList property of the existing PostJobViewModel instance
                var model = new PostJobViewModel
                {
                    PostedJobList = deletedJobs
                };

                return View(model);
            }
            else
            {
                // Handle scenario where organization ID is not found
                // For example, redirect to login or display an error message
                return RedirectToAction("ListsJobs", "Organization"); // Redirect to login page
            }
        }


        public IActionResult Restore(int id)
        {
            var data = _dbContext.PostJobs
                .Where(x => x.JobId == id).FirstOrDefault();
            if (data != null)
            {
                data.isdeleted = false;
                _dbContext.Entry(data).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            return RedirectToAction("ListsJobs");
        }

        public IActionResult MarkAsDeleted(int id)
        {
            try
            {
                var data = _dbContext.PostJobs
                    .FirstOrDefault(x => x.JobId == id && x.isdeleted);

                if (data != null)
                {
                    _dbContext.PostJobs.Remove(data);
                    _dbContext.SaveChanges();
                }

                return RedirectToAction("DeletedList");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the deletion process
                // You might want to log the exception and display an error message to the user
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        public IActionResult Details(int id)
        {
            var data = _dbContext.PostJobs.FirstOrDefault(x => x.JobId == id);

            if (data != null)
            {
                return View(data);
            }

            return RedirectToAction("ListsJobs");
        }


    }
}
    


