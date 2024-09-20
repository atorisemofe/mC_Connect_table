using Microsoft.AspNetCore.Mvc;

namespace mC_Connect_table.Models
{
    public class NotificationViewModel
{
    public string Title { get; set; }
    public string Id { get; set; }

    // For Push Switch and Image Updated
    public string? Address { get; set; }
    public string? device_name { get; set; }

    // Push Switch-specific
    public int? Action { get; set; }

    // Image Updated-specific
    public string? job_id { get; set; }

    // App Disconnection-specific
    public string? Name { get; set; }
    public bool? Intentional { get; set; }

    // Battery Capacity-specific
    public int? BatteryLevel { get; set; }
}

}
