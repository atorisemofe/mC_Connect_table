using Microsoft.AspNetCore.Mvc;

namespace mC_Connect_table.Models
{
    public class NotificationViewModel
{
    public string title { get; set; }
    public string id { get; set; }

    // For Push Switch and Image Updated
    public string? address { get; set; }
    public string? device_name { get; set; }

    // Push Switch-specific
    public int? action { get; set; }

    // Image Updated-specific
    public string? job_id { get; set; }

    // App Disconnection-specific
    public string? name { get; set; }
    public bool? intentional { get; set; }

    // Battery Capacity-specific
    public int? battery_level { get; set; }
}

}
