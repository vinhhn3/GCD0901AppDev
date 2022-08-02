using GCD0901AppDev.Data;
using GCD0901AppDev.Models;
using GCD0901AppDev.Utils;
using GCD0901AppDev.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

namespace GCD0901AppDev.Controllers
{
  [Authorize(Roles = Role.USER)]
  public class TodoesController : Controller
  {
    private ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public TodoesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }
    [HttpGet]
    public IActionResult Index(string category)
    {
      var currentUserId = _userManager.GetUserId(User);
      if (!string.IsNullOrWhiteSpace(category))
      {
        var result = _context.Todoes
          .Include(t => t.Category)
          .Where(t => t.Category.Description.Equals(category)
              && t.UserId == currentUserId
          )
          .ToList();

        return View(result);
      }

      IEnumerable<Todo> todoes = _context.Todoes
        .Include(t => t.Category)
        .Where(t => t.UserId == currentUserId)
        .ToList();
      return View(todoes);
    }

    [HttpGet]
    public IActionResult Create()
    {
      var viewModel = new TodoCategoriesViewModel()
      {
        Categories = _context.Categories.ToList()
      };
      return View(viewModel);
    }
    [HttpPost]
    public IActionResult Create(TodoCategoriesViewModel viewModel)
    {
      if (!ModelState.IsValid)
      {
        viewModel = new TodoCategoriesViewModel
        {
          Categories = _context.Categories.ToList()
        };
        return View(viewModel);
      }

      var currentUserId = _userManager.GetUserId(User);
      var newTodo = new Todo
      {
        Description = viewModel.Todo.Description,
        CategoryId = viewModel.Todo.CategoryId,
        UserId = currentUserId
      };

      _context.Add(newTodo);
      _context.SaveChanges();
      return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult Delete(int id)
    {
      var currentUserId = _userManager.GetUserId(User);
      var todoInDb = _context.Todoes.SingleOrDefault(
        t => t.Id == id && t.UserId == currentUserId);
      if (todoInDb is null)
      {
        return NotFound();
      }

      _context.Todoes.Remove(todoInDb);
      _context.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
      var currentUserId = _userManager.GetUserId(User);
      var todoInDb = _context.Todoes.SingleOrDefault(
        t => t.Id == id && t.UserId == currentUserId);
      if (todoInDb is null)
      {
        return NotFound();
      }
      var viewModel = new TodoCategoriesViewModel
      {
        Todo = todoInDb,
        Categories = _context.Categories.ToList()
      };
      return View(viewModel);
    }

    [HttpPost]
    public IActionResult Edit(TodoCategoriesViewModel viewModel)
    {
      var currentUserId = _userManager.GetUserId(User);
      var todoInDb = _context.Todoes.SingleOrDefault(
        t => t.Id == viewModel.Todo.Id && t.UserId == currentUserId);
      if (todoInDb is null)
      {
        return BadRequest();
      }

      if (!ModelState.IsValid)
      {
        viewModel = new TodoCategoriesViewModel
        {
          Todo = viewModel.Todo,
          Categories = _context.Categories.ToList()
        };
        return View(viewModel);
      }

      todoInDb.Description = viewModel.Todo.Description;
      todoInDb.Status = viewModel.Todo.Status;
      todoInDb.CategoryId = viewModel.Todo.CategoryId;

      _context.SaveChanges();

      return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
      var currentUserId = _userManager.GetUserId(User);
      var todoInDb = _context.Todoes
        .Include(t => t.Category)
        .SingleOrDefault(t => t.Id == id && t.UserId == currentUserId);
      if (todoInDb is null)
      {
        return NotFound();
      }

      return View(todoInDb);
    }

    [HttpGet]
    public IActionResult ByCategory(int id)
    {
      var categoryInDb = _context.Categories
        .Include(t => t.Todoes)
        .SingleOrDefault(t => t.Id == id);

      if (categoryInDb == null)
      {
        return NotFound();
      }
      var currentUserId = _userManager.GetUserId(User);
      var todoesByCategoryName = categoryInDb.Todoes
        .Where(t => t.UserId == currentUserId)
        .ToList();
      return View("Index", todoesByCategoryName);
    }
  }
}
