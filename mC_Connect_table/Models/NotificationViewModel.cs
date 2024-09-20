using Microsoft.AspNetCore.Mvc;

namespace mC_Connect_table.Models
{
    public class NotificationViewModel
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string Address { get; set; }
        public string Device_Name { get; set; }
        public int Action { get; set; }
    }
}
