using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharedLibrary.Models;

namespace SalesOrder.Pages
{
    public class OrderLineModel : PageModel
    {
        public OrderHeader order;
        public OrderLineModel(OrderHeader orderLine)
        {
            order = orderLine;
        }

        public void OnPost(string filter)
        {

        }
    }
}
