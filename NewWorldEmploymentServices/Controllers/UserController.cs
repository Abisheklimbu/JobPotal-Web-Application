using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewWorldEmploymentServices.AppWebDbContext;
using NewWorldEmploymentServices.Models;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewWorldEmploymentServices.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _dbContext;

        public UserController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }


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

        public IActionResult Details(int id)
        {
            // Assuming dbContext is an instance of YourDbContext
            ViewBag.DbContextInstance = _dbContext;
            var data = _dbContext.PostJobs.FirstOrDefault(x => x.JobId == id);

            if (data != null)
            {
                return View(data);
            }

            return RedirectToAction("ListJobs");
        }

        public IActionResult _Search(string search)
        {
            PostJobViewModel model = new PostJobViewModel();
            model.PostedJobList = (from i in _dbContext.PostJobs
                                      where i.isdeleted == false
                                      select new PostJobViewModel
                                      {
                                          JobName = i.JobName,
                                          JobLocation = i.JobLocation,
                                        
                                      }).ToList();

            if (search != null)
            {
                model.PostedJobList = model.PostedJobList.
                    Where(x => x.JobName.ToLower().Contains(search.ToLower())).ToList();
            }
            return View(model);
        }
    }
}
