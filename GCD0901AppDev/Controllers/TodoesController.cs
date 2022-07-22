using Microsoft.AspNetCore.Mvc;

namespace GCD0901AppDev.Controllers
{
  public class TodoesController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
