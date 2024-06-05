using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewWorldEmploymentServices.AppWebDbContext;
using NewWorldEmploymentServices.Models;

namespace NewWorldEmploymentServices.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
        {
            _logger = logger;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        // For Listing Job
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

        // For searching job
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

        // For showing details of job 
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

    }

}
