using DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Interfaces;
using SharedLibrary.Models;
using SharedLibrary.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SalesOrder.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDBService _dBService;
        private readonly IJWTWebLoginHandler _jWTWebLoginHandler;
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public Login auth;

        public IndexModel(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IDBService dBService,
            IJWTWebLoginHandler jWTWebLoginHandler,
            ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _dBService = dBService;
            _jWTWebLoginHandler = jWTWebLoginHandler;
            _logger = logger;
        }

        public void OnGet()
        {
            auth = new Login();
        }

        public async Task<IActionResult> OnPostAsync(Login auth)
        {
            User user = await _userManager.FindByEmailAsync(auth.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, auth.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(Convert.ToInt32(_configuration["AuthTokenExpiryInHours"])),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                await _jWTWebLoginHandler.Login(HttpContext, new JwtSecurityTokenHandler().WriteToken(token), user.Name, user.Surname);
            }

            return Redirect("/Orders");
        }
    }
}
