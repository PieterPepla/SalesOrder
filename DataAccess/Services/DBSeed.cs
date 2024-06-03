using DataAccess.Context;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedLibrary.Constants;
using SharedLibrary.Enums;
using SharedLibrary.Models;

namespace DataAccess.Services
{
    public class DBSeed : IDBSeed
    {
        private readonly SalesOrderDBContext _salesOrderDBContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SalesOrderDBContext> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public DBSeed(
            SalesOrderDBContext salesOrderDBContext,
            IConfiguration configuration,
            ILogger<SalesOrderDBContext> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _salesOrderDBContext = salesOrderDBContext;
            _configuration = configuration;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedDefaultData()
        {
            bool autoMigrate = bool.Parse(_configuration["ConnectionStrings:AutoMigrate"]);

            if (autoMigrate)
            {
                _salesOrderDBContext.Database.Migrate();
            }

            await SeedRoles(DefaultRoles());
            await SeedDefaultUsers(DefaultUsers());
            await SeedOrders(DefaultOrders());
        }

        private async Task SeedRoles(List<string> roles)
        {
            try
            {
                foreach (string role in roles)
                {
                    if (!_salesOrderDBContext.Roles.Any(r => r.Name.ToUpper() == role.ToUpper()))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                await _salesOrderDBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to seed default roles.");
            }
        }

        private async Task SeedDefaultUsers(List<object> users)
        {
            try
            {
                foreach (dynamic userModel in users)
                {
                    User user = (User)userModel.User;
                    string role = userModel.Role;

                    User userExists = await _userManager.FindByNameAsync(user.Email);

                    if (userExists != null)
                    {
                        continue;
                    }

                    IdentityResult result = await _userManager.CreateAsync(user, user.PasswordHash);

                    if (result.Succeeded)
                    {
                        if (await _roleManager.RoleExistsAsync(role))
                        {
                            await _userManager.AddToRoleAsync(user, role);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to seed default users.");
            }
        }

        private async Task SeedOrders(List<OrderHeader> orders)
        {
            try
            {
                foreach (dynamic orderModel in orders)
                {
                    OrderHeader order = (OrderHeader)orderModel;

                    OrderHeader orderExists = await _salesOrderDBContext.OrderHeader.Where(x => x.Id == order.Id).FirstOrDefaultAsync();

                    if (orderExists != null)
                    {
                        continue;
                    }

                    await _salesOrderDBContext.AddAsync(order);
                }
                await _salesOrderDBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to seed default orders.");
            }
        }

        private List<string> DefaultRoles()
        {
            return new List<string> {
                UserRole.Administrator,
                UserRole.Guest
            };
        }

        private List<object> DefaultUsers()
        {
            return new List<object> {
                new
                {
                    User = new User
                    {
                        Name = "Pieter",
                        Surname = "Steenekamp",
                        Email = "pietersteenekamp@steenekamp.net",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = "pietersteenekamp@steenekamp.net",
                        PasswordHash = "Password@123",
                        EmailConfirmed = true,
                    },
                    Role = UserRole.Administrator
                }
            };
        }

        private List<OrderHeader> DefaultOrders()
        {
            return new List<OrderHeader> {
                new OrderHeader
                {
                    Id = Guid.Parse("498E3D42-614D-4ABF-6D22-08DC83AFDDC5"),
                    OrderNumber = "SO625144",
                    OrderType = OrderType.Normal,
                    OrderStatus = OrderStatus.New,
                    CustomerName = "KFC",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    OrderLine = new List<OrderLine>
                    {
                        new OrderLine
                        {
                            Id = Guid.Parse("33CCCB68-526F-468B-FF12-08DC83AFDDC8"),
                            LineNumber = 1,
                            ProductCode = "GSX837420",
                            ProductType = ProductType.Parts,
                            CostPrice = decimal.Parse("13.54"),
                            SalesPrice = decimal.Parse("84.49"),
                            Quantity = 10,
                            DateCreated = DateTime.Now,
                            DateUpdated = DateTime.Now,
                        },
                        new OrderLine
                        {
                            Id = Guid.Parse("F83D8646-2ED5-4E2C-FF13-08DC83AFDDC8"),
                            LineNumber = 2,
                            ProductCode = "AVF697420",
                            ProductType = ProductType.Apparel,
                            CostPrice = decimal.Parse("19.34"),
                            SalesPrice = decimal.Parse("105.99"),
                            Quantity = 25,
                            DateCreated = DateTime.Now,
                            DateUpdated = DateTime.Now,
                        },
                    }
                },
                new OrderHeader
                {
                    Id = Guid.Parse("FE0DAB74-5D53-40C6-6D23-08DC83AFDDC5"),
                    OrderNumber = "SO625145",
                    OrderType = OrderType.Normal,
                    OrderStatus = OrderStatus.New,
                    CustomerName = "Nandos",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    OrderLine = new List<OrderLine>
                    {
                        new OrderLine
                        {
                            Id = Guid.Parse("04DE6F07-F4CB-4757-FF14-08DC83AFDDC8"),
                            LineNumber = 1,
                            ProductCode = "YTS786663",
                            ProductType = ProductType.Parts,
                            CostPrice = decimal.Parse("8.44"),
                            SalesPrice = decimal.Parse("49.22"),
                            Quantity = 2,
                            DateCreated = DateTime.Now,
                            DateUpdated = DateTime.Now,
                        },
                        new OrderLine
                        {
                            Id = Guid.Parse("AC73B395-FD50-408B-FF15-08DC83AFDDC8"),
                            LineNumber = 2,
                            ProductCode = "UYT485911",
                            ProductType = ProductType.Equipment,
                            CostPrice = decimal.Parse("41.57"),
                            SalesPrice = decimal.Parse("250.45"),
                            Quantity = 12,
                            DateCreated = DateTime.Now,
                            DateUpdated = DateTime.Now,
                        },
                        new OrderLine
                        {
                            Id = Guid.Parse("6BEDE260-9519-4957-FF16-08DC83AFDDC8"),
                            LineNumber = 3,
                            ProductCode = "PLO879996",
                            ProductType = ProductType.Apparel,
                            CostPrice = decimal.Parse("21.57"),
                            SalesPrice = decimal.Parse("65.45"),
                            Quantity = 8,
                            DateCreated = DateTime.Now,
                            DateUpdated = DateTime.Now,
                        },
                    }
                }
            };
        }
    }
}
