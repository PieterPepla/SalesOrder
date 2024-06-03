using SharedLibrary.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models
{
    public class OrderLine : BaseModel
    {
        public int LineNumber { get; set; }

        [Required]
        public string ProductCode { get; set; }

        [Required]
        public ProductType ProductType { get; set; }

        [Required]
        public decimal CostPrice { get; set; }

        [Required]
        public decimal SalesPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [ForeignKey("OrderHeaderId")]
        public OrderHeader OrderHeader { get; set; }
    }
}
