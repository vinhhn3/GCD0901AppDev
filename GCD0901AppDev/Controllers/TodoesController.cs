using GCD0901AppDev.Data;
using GCD0901AppDev.Models;
using GCD0901AppDev.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    [HttpGet]
    public IActionResult Index()
    {
      IEnumerable<Todo> todoes = _context.Todoes
        .Include(t => t.Category)
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
      var newTodo = new Todo
      {
        Description = viewModel.Todo.Description,
        CategoryId = viewModel.Todo.CategoryId
      };

      _context.Add(newTodo);
      _context.SaveChanges();
      return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult Delete(int id)
    {
      var todoInDb = _context.Todoes.SingleOrDefault(t => t.Id == id);
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
      var todoInDb = _context.Todoes.SingleOrDefault(t => t.Id == id);
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
      var todoInDb = _context.Todoes.SingleOrDefault(t => t.Id == viewModel.Todo.Id);
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
      var todoInDb = _context.Todoes
        .Include(t => t.Category)
        .SingleOrDefault(t => t.Id == id);
      if (todoInDb is null)
      {
        return NotFound();
      }

      return View(todoInDb);
    }
  }
}
