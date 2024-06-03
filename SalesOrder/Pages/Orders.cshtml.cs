using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharedLibrary.Enums;
using SharedLibrary.Models;

namespace SalesOrder.Pages
{
    [Authorize]
    public class AuthModel : PageModel
    {
        private readonly IDBService _dBService;
        private readonly ILogger<AuthModel> _logger;

        public List<OrderHeader> orders;

        public AuthModel(IDBService dBService, ILogger<AuthModel> logger)
        {
            _dBService = dBService;
            _logger = logger;
        }

        public void OnGet(Guid? id)
        {
            if (id != null)
                _dBService.DeleteOrderHeader(_dBService.SelectOrderHeader(id).FirstOrDefault());

            orders = _dBService.SelectOrderHeader();
        }

        public void OnPost(OrderType filter)
        {
            orders = _dBService.FilterOrderHeader(filter);
        }
    }
}
