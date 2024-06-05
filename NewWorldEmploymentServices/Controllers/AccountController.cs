using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NewWorldEmploymentServices.AppWebDbContext;
using NewWorldEmploymentServices.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using NewWorldEmploymentServices.EntityModels;

namespace NewWorldEmploymentServices.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _dbContext;

        public AccountController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Check if both email and password are provided
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("", "Email and Password are required");
                return View(model); // Return the login view with validation errors
            }

            // If both email and password are provided, proceed with the login process
            if (ModelState.IsValid)
            {
                var organization = _dbContext.Organizations.FirstOrDefault(o => o.Email == model.Email);
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == model.Email);
                var admin = _dbContext.Admins.FirstOrDefault(a => a.Email == model.Email);

                if (organization != null || user != null || admin != null)
                {
                    // Check if the password matches for the found user type
                    if ((organization != null && organization.Password == model.Password) ||
                        (user != null && user.Password == model.Password) ||
                        (admin != null && admin.Password == model.Password))
                    {
                        // Create claims for the authenticated user
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.Email)
                            // You can add more claims if needed
                        };

                        // Create identity for the user
                        var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        
                        // Create authentication properties
                        var authProperties = new AuthenticationProperties
                        {
                            // Configure properties, such as whether to persist the authentication
                            IsPersistent = true
                        };

                        // Sign in the user
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(userIdentity),
                            authProperties);

                        // Store the organization ID in a session variable
                        if (organization != null)
                        {
                            HttpContext.Session.SetInt32("OrganizationId", organization.OrganizationId);
                        }


                        // Redirect to the corresponding dashboard
                        if (organization != null)

                            return RedirectToAction("OrganizationDashboard");

                        else if (user != null)
                            return RedirectToAction("UserDashboard");

                        else if (admin != null)
                            return RedirectToAction("AdminDashboard");
                    }
                    else
                    {
                        // Password does not match
                        ModelState.AddModelError("", "Invalid password.");
                    }
                }
                else
                {
                    // Email does not exist in the database
                    ModelState.AddModelError("", "Invalid email.");
                }
            }

            // If ModelState is not valid or login failed, return the login view with validation errors
            return View("~/Views/Account/Login.cshtml", model);
        }


        // Organization Dashboard
        public IActionResult OrganizationDashboard()
        {
            return RedirectToAction("Index", "Organization");
        }

        // User Dashboard
        public IActionResult UserDashboard()
        {
            return RedirectToAction("Index", "User");
        }

        // Admin Dashboard
        public IActionResult AdminDashboard()
        {
            return RedirectToAction("DashBoard", "Admin");
        }



        // GET: /Account/UserRegistration
        public IActionResult UserRegistration()
        {
            return View();
        }

        // POST: /Account/UserRegistration
        [HttpPost]
        public IActionResult UserRegistration(UserRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = _dbContext.Users.Where(x => x.Email == model.Email).FirstOrDefault();
                if (data != null)
                {
                    ModelState.AddModelError("Error", "Email already exist");
                    return View(model);
                }
                else
                {
                    User entity = new User();
                    entity.Name = model.Name;
                    entity.Email = model.Email;
                    entity.Password = model.Password;
                    _dbContext.Users.Add(entity);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Login");
                }
            }
            return View();
          
        }

        // GET: /Account/OrganizationRegistration
        public IActionResult OrganizationRegistration()
        {
            return View();
        }

        // POST: /Account/OrganizationRegistration
        [HttpPost]
        public IActionResult OrganizationRegistration(OrganizationRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = _dbContext.Organizations.Where(x => x.Email == model.Email).FirstOrDefault();
                if (data != null)
                {
                    ModelState.AddModelError("Error", "Email already exist");
                    return View(model);
                }
                else
                {
                    Organization entity = new Organization();
                    entity.Name = model.Name;
                    entity.Email = model.Email;
                    entity.Password = model.Password;
                    _dbContext.Organizations.Add(entity);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Login");
                }
            }
            return View();  
        }



        // GET: Account/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return View("Logout", "Account");
            }
            else 
            // If the user is not authenticated, redirect to the login page
            return RedirectToAction("Login", "Account");
        }

        // POST: Account/LogoutConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutConfirmed()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the Home page
            return RedirectToAction("Index", "Home");
        }
    
    }
}
