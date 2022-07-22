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
    [HttpGet]
    public IActionResult Index()
    {
      IEnumerable<Todo> todoes = _context.Todoes.ToList();
      return View(todoes);
    }

    [HttpGet]
    public IActionResult Create()
    {
      return View();
    }
    [HttpPost]
    public IActionResult Create(Todo todo)
    {
      var newTodo = new Todo
      {
        Description = todo.Description
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

      return View(todoInDb);
    }

    [HttpPost]
    public IActionResult Edit(Todo todo)
    {
      var todoInDb = _context.Todoes.SingleOrDefault(t => t.Id == todo.Id);
      if (todoInDb is null)
      {
        return BadRequest();
      }

      todoInDb.Description = todo.Description;
      todoInDb.Status = todo.Status;

      _context.SaveChanges();

      return RedirectToAction("Index");
    }
  }
}
