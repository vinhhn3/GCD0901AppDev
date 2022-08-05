using GCD0901AppDev.DTOs.Requests;
using GCD0901AppDev.DTOs.Responses;
using GCD0901AppDev.Models;
using GCD0901AppDev.Repositories.Interfaces;
using GCD0901AppDev.Utils;

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
      var viewModel = new CreateTodoRequest()
      {
        Categories = _categoryRepos.GetAll()
      };
      return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateTodoRequest model)
    {
      if (!ModelState.IsValid)
      {
        model = new CreateTodoRequest
        {
          Categories = _categoryRepos.GetAll()
        };
        return View(model);
      }

      var currentUserId = _userManager.GetUserId(User);

      bool isCreated = await _todoRepos.CreateTodo(model, currentUserId);

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
      var model = new EditTodoRequest
      {
        Id = todoInDb.Id,
        Description = todoInDb.Description,
        CategoryId = todoInDb.CategoryId,
        Status = todoInDb.Status,
        Categories = _categoryRepos.GetAll()
      };
      return View(model);
    }

    [HttpPost]
    public IActionResult Edit(EditTodoRequest model)
    {
      if (!ModelState.IsValid)
      {
        model = new EditTodoRequest
        {
          Id = model.Id,
          Description = model.Description,
          CategoryId = model.CategoryId,
          Status = model.Status,
          Categories = _categoryRepos.GetAll()
        };
        return View(model);
      }
      var currentUserId = _userManager.GetUserId(User);
      var isEdited = _todoRepos.EditTodo(model, currentUserId);

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

      var response = new DetailsTodoResponse
      {
        Description = todoInDb.Description,
        CreatedAt = todoInDb.CreatedAt,
        Status = todoInDb.Status,
        CategoryDescription = todoInDb.Category.Description,
        ImageUrl = ConvertByteArrayToStringBase64(todoInDb.ImageData)
      };

      return View(response);
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

    [NonAction]
    private string ConvertByteArrayToStringBase64(byte[] imageArray)
    {
      string imageBase64Data = Convert.ToBase64String(imageArray);

      return string.Format("data:image/jpg;base64, {0}", imageBase64Data);
    }
  }
}
