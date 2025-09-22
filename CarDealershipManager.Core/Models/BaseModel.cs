using System.ComponentModel.DataAnnotations;

namespace CarDealershipManager.Core.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        
        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }

        [ScaffoldColumn(false)]
        public DateTime UpdatedAt { get; set; }
        
        [ScaffoldColumn(false)]
        public bool IsDeleted { get; set; } = false;
    }
}
