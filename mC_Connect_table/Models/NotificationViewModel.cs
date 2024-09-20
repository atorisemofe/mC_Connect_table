using Microsoft.AspNetCore.Mvc;

namespace mC_Connect_table.Models
{
    public class NotificationViewModel
{
    public string Title { get; set; }
    public string Id { get; set; }

    // For Push Switch and Image Updated
    public string Address { get; set; }
    public string DeviceName { get; set; }

    // Push Switch-specific
    public int? Action { get; set; }

    // Image Updated-specific
    public string JobId { get; set; }

    // App Disconnection-specific
    public string Name { get; set; }
    public bool? Intentional { get; set; }

    // Battery Capacity-specific
    public int? BatteryLevel { get; set; }
}

}
