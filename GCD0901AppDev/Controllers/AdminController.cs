using GCD0901AppDev.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GCD0901AppDev.Controllers
{
  [Authorize(Roles = Role.ADMIN)]
  public class AdminController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
