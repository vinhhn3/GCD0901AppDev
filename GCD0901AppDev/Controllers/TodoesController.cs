using GCD0901AppDev.Models;
using GCD0901AppDev.Repositories.Interfaces;
using GCD0901AppDev.Utils;
using GCD0901AppDev.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCD0901AppDev.Controllers
{
  [Authorize(Roles = Role.USER)]
  public class TodoesController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private ITodoRepository _todoRepos;
    private ICategoryRepository _categoryRepos;
    public TodoesController(
      UserManager<ApplicationUser> userManager,
      ITodoRepository todoRepos,
      ICategoryRepository categoryRepos
      )
    {
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
          .GetAll(currentUserId, category);

        return View(result);
      }

      IEnumerable<Todo> todoes = _todoRepos
        .GetAll(currentUserId);
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

      bool isCreated = await _todoRepos.CreateTodo(viewModel, currentUserId);

      if (!isCreated) return BadRequest();
      return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult Delete(int id)
    {
      var currentUserId = _userManager.GetUserId(User);
      var isDeleted = _todoRepos.DeleteTodo(id, currentUserId);

      if (!isDeleted) return NotFound();
      return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
      var currentUserId = _userManager.GetUserId(User);
      var todoInDb = _todoRepos.GetTodo(id, currentUserId);
      if (todoInDb is null)
      {
        return NotFound();
      }
      var viewModel = new TodoCategoriesViewModel
      {
        Todo = todoInDb,
        Categories = _categoryRepos.GetAll()
      };
      return View(viewModel);
    }

    [HttpPost]
    public IActionResult Edit(TodoCategoriesViewModel viewModel)
    {
      if (!ModelState.IsValid)
      {
        viewModel = new TodoCategoriesViewModel
        {
          Todo = viewModel.Todo,
          Categories = _categoryRepos.GetAll()
        };
        return View(viewModel);
      }
      var currentUserId = _userManager.GetUserId(User);
      var isEdited = _todoRepos.EditTodo(viewModel, currentUserId);

      if (!isEdited) return BadRequest();
      return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
      var currentUserId = _userManager.GetUserId(User);
      var todoInDb = _todoRepos.GetTodo(id, currentUserId);
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
      var todoesByCategoryName = GetTodoesFromCategoryAndUserId(categoryInDb, currentUserId);
      return View("Index", todoesByCategoryName);
    }

    [NonAction]
    private List<Todo> GetTodoesFromCategoryAndUserId(Category category, string userId)
    {
      return category.Todoes
        .Where(t => t.UserId == userId)
        .ToList();
    }
  }
}
