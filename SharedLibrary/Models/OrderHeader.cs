using SharedLibrary.Enums;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class OrderHeader : BaseModel
    {
        [Required]
        public string OrderNumber { get; set; }

        [Required]
        public OrderType OrderType { get; set; }

        [Required]
        public OrderStatus OrderStatus { get; set; }

        [Required]
        public string CustomerName { get; set; }
        public List<OrderLine> OrderLine { get; set; }
    }
}
