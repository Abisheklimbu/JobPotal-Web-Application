using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewWorldEmploymentServices.AppWebDbContext;
using NewWorldEmploymentServices.EntityModels;
using NewWorldEmploymentServices.Models;
using System;
using System.Linq;

namespace NewWorldEmploymentServices.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _dbContext;

        public AdminController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AdminProfile()
        {
            return View();
        }
        public IActionResult DashBoard()
        {
            return View();
        }

        //For viewing jobs lsits by admin 
        public IActionResult ListJobs(string search)
        {
            PostJobViewModel model = new PostJobViewModel();
            model.PostedJobList = (from i in _dbContext.PostJobs
                                   where i.isdeleted == false
                                   select new PostJobViewModel
                                   {
                                       JobId = i.JobId,
                                       JobName = i.JobName,
                                       JobLocation = i.JobLocation,

                                   }).ToList();

            if (search != null)
            {
                model.PostedJobList = model.PostedJobList.Where(x => x.JobName.ToLower().Contains(search.ToLower())).ToList();
            }
            return View(model);
        }

        // For editing by admin
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
                model.ContactNo = data.ContactNo;
                model.Email = data.Email;
                model.Description = data.Description;

            }

            // You may want to populate ViewBag.Categories and ViewBag.Natures here
            // Populate ViewBag.Categories
            ViewBag.Categories = _dbContext.JobCategories
                                    .Select(c => new SelectListItem { Value = c.JobCategoryId.
                                    ToString(), Text = c.Name })
                                    .ToList();

            // Populate ViewBag.Natures
            ViewBag.Natures = _dbContext.JobNatures
                                    .Select(n => new SelectListItem { Value = n.JobNatureId.
                                    ToString(), Text = n.Name })
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
                // Handle the case where the job is not found such as a 404 Not Found
                return NotFound();
            }
        }
        public IActionResult Delete(int id)
        {
            var data = _dbContext.PostJobs
                .Where(x => x.JobId == id).FirstOrDefault();
            if (data != null)
            {
                data.isdeleted = true;
                _dbContext.Entry(data).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            return RedirectToAction("DeletedList");
        }

        public IActionResult DeletedList()
        {
            // Retrieve all deleted jobs
            var deletedJobs = _dbContext.PostJobs
                .Where(job => job.isdeleted)
                .Select(job => new PostJobViewModel
                {
                    JobId = job.JobId,
                    JobName = job.JobName,
                    JobLocation = job.JobLocation
                    
                })
                .ToList();

            // Populate the PostedJobList property of the PostJobViewModel instance
            var model = new PostJobViewModel
            {
                PostedJobList = deletedJobs
            };

            return View(model);

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
            return RedirectToAction("ListJobs");
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

            return RedirectToAction("ListJobs");
        }


        public IActionResult ViewUsersDetails()
        {
            var users = _dbContext.Users.ToList(); // Assuming your users table is named "Users"

            var viewModel = new UserViewModel
            {
                Users = users
            };

            return View(viewModel);
        }

        public IActionResult ViewOrganizationsDetails()
        {
            var organizations = _dbContext.Organizations.ToList(); // Assuming your organizations table is named "Organizations"

            var viewModel = new OrganizationViewModel
            {
                Organizations = organizations
            };

            return View(viewModel);
        }

        public IActionResult ViewAdminsDetails()
        {
            var admins = _dbContext.Admins.ToList(); // Assuming your admins table is named "Admins"

            var viewModel = new AdminViewModel
            {
                Admins = admins
            };

            return View(viewModel);
        }



        public IActionResult DeleteOrganization(int id)
        {
            var data = _dbContext.Organizations
                .Where(x => x.OrganizationId == id).FirstOrDefault();
            if (data != null)
            {
                data.isdeleted = true;
                _dbContext.Entry(data).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            return RedirectToAction("DeletedListOrganization");
        }

        public IActionResult DeletedListOrganization()
        {
            // Retrieve all deleted organizations
            var deletedOrganizations = _dbContext.Organizations
                .Where(org => org.isdeleted)
                .Select(org => new OrganizationViewModel
                {
                    OrganizationId = org.OrganizationId,
                    Name = org.Name,
                    Email = org.Email
                    // Add other properties as needed
                })
                .ToList();

            // Create an instance of OrganizationViewModel and assign the list of deleted organizations
            var model = new OrganizationViewModel
            {
                OrganizationsList = deletedOrganizations
            };

            return View(model);
        }

        public IActionResult RestoreOrganization(int id)
        {
            var data = _dbContext.Organizations
                .Where(x => x.OrganizationId == id).FirstOrDefault();
            if (data != null)
            {
                data.isdeleted = false;
                _dbContext.Entry(data).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            return RedirectToAction("ViewOrganizationsDetails");
        }

        public IActionResult MarkAsDeletedOrganization(int id)
        {
            try
            {
                var data = _dbContext.Organizations.FirstOrDefault(x => x.OrganizationId == id);

                if (data != null)
                {
                    data.isdeleted = true; // Mark as deleted
                    _dbContext.Entry(data).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }

                return RedirectToAction("DeletedListOrganization");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the deletion process
                // You might want to log the exception and display an error message to the user
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }




        //For User

        public IActionResult DeleteUser(int id)
        {
            var data = _dbContext.Users
                .Where(x => x.UserId == id).FirstOrDefault();
            if (data != null)
            {
                data.isdeleted = true;
                _dbContext.Entry(data).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            return RedirectToAction("DeletedListUser");
        }

        public IActionResult DeletedListUser()
        {
            // Retrieve all deleted organizations
            var deletedUsers = _dbContext.Users
                .Where(org => org.isdeleted)
                .Select(org => new UserViewModel
                {
                    UserId= org.UserId,
                    Name = org.Name,
                    Email = org.Email
                    // Add other properties as needed
                })
                .ToList();

            // Create an instance of OrganizationViewModel and assign the list of deleted organizations
            var model = new UserViewModel
            {
                UsersList = deletedUsers
            };

            return View(model);
        }

        public IActionResult RestoreUser(int id)
        {
            var data = _dbContext.Users
                .Where(x => x.UserId == id).FirstOrDefault();
            if (data != null)
            {
                data.isdeleted = false;
                _dbContext.Entry(data).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            return RedirectToAction("ViewUsersDetails");
        }

        public IActionResult MarkAsDeletedUser(int id)
        {
            try
            {
                var data = _dbContext.Users.FirstOrDefault(x => x.UserId == id);

                if (data != null)
                {
                    data.isdeleted = true; // Mark as deleted
                    _dbContext.Entry(data).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }

                return RedirectToAction("DeletedListUser");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the deletion process
                // You might want to log the exception and display an error message to the user
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        //For Admin
        // GET: For fetching admin details to edit
        public IActionResult EditAdminDetails(int id)
        {
            AdminViewModel model = new AdminViewModel();
            var data = _dbContext.Admins.FirstOrDefault(x => x.AdminId == id);
            if (data != null)
            {
                model.AdminId = data.AdminId;
                model.Email = data.Email;
                model.Username = data.Username;
                model.Password = data.Password;
            }
            return View(model);
        }

        // POST: For handling the edited admin details form submission
        [HttpPost]
        public IActionResult EditAdminDetails(AdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }

            // Update the admin details in the database
            var existingAdmin = _dbContext.Admins.FirstOrDefault(x => x.AdminId == model.AdminId);
            if (existingAdmin != null)
            {
                existingAdmin.Email = model.Email;
                existingAdmin.Username = model.Username;
                existingAdmin.Password = model.Password;
                _dbContext.SaveChanges();
            }
            else
            {
                // Handle the case where the admin is not found
                // Return an appropriate response, such as a 404 Not Found
                return NotFound();
            }

            // Redirect to the view admin details page after successful update
            return RedirectToAction("ViewAdminsDetails", "Admin");
        }

    }
}




