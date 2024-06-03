using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SharedLibrary.Constants;
using SharedLibrary.Models;
using SharedLibrary.ViewModels;
using System.Data;

namespace SalesOrder.Pages
{
    [Authorize(Roles = $"{UserRole.Administrator}")]
    public class AddUserModel : PageModel
    {
        private readonly IDBService _dBService;
        private readonly ILogger<AddUserModel> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public readonly UserVM user = new UserVM();
        public List<string> messages = new List<string>();

        public AddUserModel(IDBService dBService, ILogger<AddUserModel> logger,
            RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _dBService = dBService;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task OnPostAsync(UserVM user)
        {
            User newUser = new User
            {
                Email = user.Username,
                PasswordHash = user.Password,
                UserName = user.Username,
                Name = "Name",
                Surname = "Surname"
            };
            IdentityResult result = await _userManager.CreateAsync(newUser, newUser.PasswordHash);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, user.Role);
                messages.Add("Success");
            }
            else
            {
                foreach(var message in result.Errors)
                {
                    messages.Add(message.Description);
                }
            }
        }
    }
}
