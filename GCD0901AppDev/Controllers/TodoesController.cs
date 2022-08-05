using GCD0901AppDev.Data;
using GCD0901AppDev.Models;
using GCD0901AppDev.Repositories.Interfaces;
using GCD0901AppDev.Utils;
using GCD0901AppDev.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GCD0901AppDev.Controllers
{
  [Authorize(Roles = Role.USER)]
  public class TodoesController : Controller
  {
    private ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private ITodoRepository _todoRepos;
    private ICategoryRepository _categoryRepos;
    public TodoesController(
      ApplicationDbContext context,
      UserManager<ApplicationUser> userManager,
      ITodoRepository todoRepos,
      ICategoryRepository categoryRepos
      )
    {
      _context = context;
      _userManager = userManager;
      _todoRepos = todoRepos;
      _categoryRepos = categoryRepos;
    }
    [HttpGet]
    public IActionResult Index(string category)
    {
      var currentUserId = _userManager.GetUserId(User);
      if (!string.IsNullOrWhiteSpace(category))
      {
        var result = _todoRepos
          .GetAll()
          .Where(t => t.Category.Description.Equals(category)
            && t.UserId == currentUserId)
          .ToList();

        return View(result);
      }

      IEnumerable<Todo> todoes = _todoRepos
        .GetAll()
        .Where(t => t.UserId == currentUserId)
        .ToList();
      return View(todoes);
    }

    [HttpGet]
    public IActionResult Create()
    {
      var viewModel = new TodoCategoriesViewModel()
      {
        Categories = _categoryRepos.GetAll()
      };
      return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> Create(TodoCategoriesViewModel viewModel)
    {
      if (!ModelState.IsValid)
      {
        viewModel = new TodoCategoriesViewModel
        {
          Categories = _categoryRepos.GetAll()
        };
        return View(viewModel);
      }

      var currentUserId = _userManager.GetUserId(User);

      using (var memoryStream = new MemoryStream())
      {
        await viewModel.FormFile.CopyToAsync(memoryStream);
        var newTodo = new Todo
        {
          Description = viewModel.Todo.Description,
          CategoryId = viewModel.Todo.CategoryId,
          UserId = currentUserId,
          ImageData = memoryStream.ToArray()
        };
        _context.Add(newTodo);
        await _context.SaveChangesAsync();
      }

      return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult Delete(int id)
    {
      var currentUserId = _userManager.GetUserId(User);
      var isDeleted = _todoRepos.DeleteByIdAndUserId(id, currentUserId);

      if (!isDeleted) return NotFound();
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
      var todoInDb = _todoRepos.GetByTodoIdAndUserId(id, currentUserId);
      if (todoInDb is null)
      {
        return NotFound();
      }

      string imageBase64Data = Convert.ToBase64String(todoInDb.ImageData);

      string image = string.Format("data:image/jpg;base64, {0}", imageBase64Data);
      ViewBag.ImageData = image;

      return View(todoInDb);
    }

    [HttpGet]
    public IActionResult ByCategory(int id)
    {

      var categoryInDb = _categoryRepos.GetById(id);

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
