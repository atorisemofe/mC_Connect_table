using System.ComponentModel.DataAnnotations;

namespace mC_Connect_table.Models
{
    public class RestaurantTables
    {
        [Key]
        public int TableNumber { get; set; }

        public bool IsActive { get; set; } = false;

        public string MctId { get; set; }
        public string? CurrentSessionId { get; set; } // Store the session ID when the table is occupied
        public string? CustomerName { get; set; }  // Optional: track customer name
    }
}