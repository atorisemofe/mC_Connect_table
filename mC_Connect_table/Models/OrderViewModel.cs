using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mC_Connect_table.Models
{
    public class OrderViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderNumber { get; set; }
        public int TableNumber { get; set; }
        public string MctId { get; set; } //MCT ID assigned to the table that placed the order
        public string Status { get; set; }  // "New", "Preparing", "Ready", "Delivered"
        public List<string>? EntreeItems { get; set; }  // List of Entree Items

        public List<string>? DrinkItems { get; set; }  // List of Drink Items

        public List<string>? DessertItems { get; set; }  // List of Dessert Items

        public string? Notes { get; set; } //Order Notes like allergies.

    }

}