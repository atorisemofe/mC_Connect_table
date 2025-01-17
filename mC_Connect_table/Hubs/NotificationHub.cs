using Microsoft.AspNetCore.SignalR;

namespace mC_Connect_table.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(NotificationMessage message)
    {
        // Broadcast the message to all connected clients
        await Clients.All.SendAsync("ReceiveNotification", message);
    }
    }
    public class NotificationMessage
    {
        public string Id { get; set; }
        // public int TableId { get; set; }
        public string Action { get; set; }  // e.g., "order_ready", "help_requested", etc.
    }
}