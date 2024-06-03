using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class BaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
    }
}
