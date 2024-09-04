using Microsoft.AspNetCore.Mvc;

public class NotificationsController : Controller
{
    public async Task<IActionResult> Index()
    {
        var title = TempData["NotificationTitle"] as string;
        var id = TempData["NotificationId"] as string;
        //if (title != null)
        //{
            var model = new mC_Connect_table.Models.NotificationViewModel
            {
                Title = title,
                Id = id
            };
            return View(model);
        //}
        //return RedirectToAction("Error", "Home"); 
    }
}


