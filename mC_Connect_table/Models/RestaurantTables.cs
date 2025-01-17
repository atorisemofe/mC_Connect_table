using System.ComponentModel.DataAnnotations;

namespace mC_Connect_table.Models
{
    public class RestaurantTables
    {
        [Key]
        public int TableNumber { get; set; }

        public bool IsActive { get; set; } = false;

        public string MctId { get; set; }
    }
}