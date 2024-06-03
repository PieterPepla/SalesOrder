using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharedLibrary.Interfaces;
using SharedLibrary.Models;

namespace SalesOrder.Pages
{
    public class OrderDetailsModel : PageModel
    {
        private IDBService _dBService;
        private readonly IXMLDBService _xMLDBService;
        private readonly ILogger<IndexModel> _logger;
        public List<OrderHeader> _xmlOrders;

        public OrderHeader order;

        public OrderDetailsModel(IDBService dBService, IXMLDBService xMLDBService, ILogger<IndexModel> logger)
        {
            _dBService = dBService;
            _xMLDBService = xMLDBService;
            _logger = logger;
            _xmlOrders = _xMLDBService.SelectOrders();
        }

        public void OnGet(Guid? id = null)
        {
            if (id != null)
                order = _dBService.SelectOrderHeader(id).FirstOrDefault();

            if (order == null)
                order = new OrderHeader
                {
                    OrderLine = new List<OrderLine>
                    {
                        new OrderLine{
                            LineNumber = 1,
                        }
                    }
                };
        }

        public IActionResult OnPost(OrderHeader order)
        {
            order.DateUpdated = DateTime.Now;
            foreach(OrderLine line in order.OrderLine)
            {
                line.DateUpdated = DateTime.Now;
            }

            var index = _xmlOrders.FindIndex(x => x.Id == order.Id);
            if (index != -1)
                _xmlOrders[index] = order;

            var xmlResult = _xMLDBService.UpdateOrders(_xmlOrders);
            var result = _dBService.UpdateOrderHeader(order);
            return Redirect("/Orders");
        }
    }
}
