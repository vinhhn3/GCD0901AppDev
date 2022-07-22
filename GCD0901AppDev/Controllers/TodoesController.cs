using GCD0901AppDev.Data;
using GCD0901AppDev.Models;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;

namespace GCD0901AppDev.Controllers
{
  public class TodoesController : Controller
  {
    private ApplicationDbContext _context;
    public TodoesController(ApplicationDbContext context)
    {
      _context = context;
    }

    public IActionResult Index()
    {
      IEnumerable<Todo> todoes = _context.Todoes.ToList();
      return View(todoes);
    }
  }
}
