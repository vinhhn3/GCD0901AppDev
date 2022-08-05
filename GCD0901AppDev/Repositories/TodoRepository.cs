using GCD0901AppDev.Data;
using GCD0901AppDev.Models;
using GCD0901AppDev.Repositories.Interfaces;
using GCD0901AppDev.ViewModels;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GCD0901AppDev.Repositories
{
  public class TodoRepository : ITodoRepository
  {
    private ApplicationDbContext _context;
    public TodoRepository(ApplicationDbContext context)
    {
      _context = context;
    }


    public async Task<bool> CreateTodoWithUserId(TodoCategoriesViewModel viewModel, string userId)
    {
      int result;
      using (var memoryStream = new MemoryStream())
      {
        await viewModel.FormFile.CopyToAsync(memoryStream);
        var newTodo = new Todo
        {
          Description = viewModel.Todo.Description,
          CategoryId = viewModel.Todo.CategoryId,
          UserId = userId,
          ImageData = memoryStream.ToArray()
        };
        _context.Add(newTodo);
        result = await _context.SaveChangesAsync();
      }
      return result > 0;

    }

    public bool DeleteByIdAndUserId(int id, string userId)
    {
      var todoInDb = GetByTodoIdAndUserId(id, userId);
      if (todoInDb == null) return false;

      _context.Todoes.Remove(todoInDb);
      _context.SaveChanges();
      return true;
    }

    public bool EditTodo(TodoCategoriesViewModel viewModel)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerable<Todo> GetAll()
    {
      return _context.Todoes
        .Include(t => t.Category)
        .ToList();
    }

    public Todo GetById(int id)
    {
      return _context.Todoes
        .Include(t => t.Category)
        .SingleOrDefault(t => t.Id == id);
    }

    public Todo GetByTodoIdAndUserId(int id, string userId)
    {
      return _context.Todoes
        .Include(t => t.Category)
        .SingleOrDefault(t => t.Id == id && t.UserId == userId);
    }

    public IEnumerable<Todo> GetTodoesByCategoryId(int id)
    {
      throw new System.NotImplementedException();
    }
  }
}
