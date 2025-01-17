using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace mC_Connect_table.Controllers
{
    public class MenuController : Controller
    {
         public IActionResult Index()
        {
            return View();
        }
    }
}