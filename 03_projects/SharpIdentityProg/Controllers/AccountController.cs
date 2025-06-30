using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharpIdentityProg.Data;
using SharpIdentityProg.Models;
using SharpIdentityProg.Services;

namespace SharpIdentityProg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IServiceProvider _servicesProvider;
        private bool _isInit;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            IServiceProvider serviceProvider)
        {
            _servicesProvider = serviceProvider;
            this.userManager = userManager;
            Init(serviceProvider).Wait();
        }

        private async Task Init(
            IServiceProvider serviceProvider)
        {
            if (_isInit) return;
            
            ApplicationDbContext _dbContext =_servicesProvider
                .GetRequiredService<ApplicationDbContext>();
            List<string> pending = _dbContext.Database
                .GetPendingMigrations()
                .ToList();
            if (pending.Any())
            {
                _dbContext.Database.Migrate();
            }
            
            using (var scope = serviceProvider.CreateScope())
            {
                SeedData.InitializeAsync(scope.ServiceProvider);
            }
            
            _isInit = true;
        }

        [HttpGet]
        public IActionResult Welcome()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok("You are NOT authenticated");
            }

            return Ok("You are authenticated");
        }


        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return BadRequest();
            }

            var userRoles = await userManager.GetRolesAsync(currentUser);

            var userProfile = new UserProfile
            {
                Id = currentUser.Id,
                Name = currentUser.UserName ?? "",
                Email = currentUser.Email ?? "",
                PhoneNumber = currentUser.PhoneNumber ?? "",

                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Address = currentUser.Address,
                CreatedAt = currentUser.CreatedAt,

                Role = string.Join(",", userRoles)
            };


            return Ok(userProfile);
        }



        [HttpPost("/signup")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            // create a new account and authenticate the user
            var user = new ApplicationUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email, // UserName will be used to authenticate the user
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address ?? "",
                CreatedAt = DateTime.Now,
            };


            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                // successful user registration
                await userManager.AddToRoleAsync(user, "client");

                return Ok();
            }



            // registration failed => show registration errors
            var errorDictionary = new Dictionary<string, string[]>();
            foreach (var error in result.Errors)
            {
                errorDictionary[error.Code] = [error.Description];
            }

            var response = new ValidationProblemDetails(errorDictionary);
            return BadRequest(response);
        }


        [Authorize(Roles = "admin")]
        [HttpGet("AdminRoute")]
        public IActionResult AdminRoute()
        {
            return Ok("Hello Admin");
        }

        [Authorize(Roles = "client")]
        [HttpGet("ClientRoute")]
        public IActionResult ClientRoute()
        {
            return Ok("Hello Client");
        }
    }
}
