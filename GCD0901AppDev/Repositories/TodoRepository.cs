using GCD0901AppDev.Data;
using GCD0901AppDev.DTOs.Requests;
using GCD0901AppDev.Models;
using GCD0901AppDev.Repositories.Interfaces;

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

    public async Task<bool> CreateTodo(CreateTodoRequest model, string userId)
    {
      int result;
      using (var memoryStream = new MemoryStream())
      {
        await model.FormFile.CopyToAsync(memoryStream);
        var newTodo = new Todo
        {
          Description = model.Description,
          CategoryId = model.CategoryId,
          UserId = userId,
          ImageData = memoryStream.ToArray()
        };
        _context.Add(newTodo);
        result = await _context.SaveChangesAsync();
      }
      return result > 0;

    }

    public bool DeleteTodo(int id, string userId)
    {
      var todoInDb = GetTodo(id, userId);
      if (todoInDb == null) return false;

      _context.Todoes.Remove(todoInDb);
      _context.SaveChanges();
      return true;
    }

    public bool EditTodo(EditTodoRequest model, string userId)
    {
      var todoInDb = GetTodo(model.Id, userId);
      if (todoInDb == null) return false;

      todoInDb.Description = model.Description;
      todoInDb.Status = model.Status;
      todoInDb.CategoryId = model.CategoryId;

      return _context.SaveChanges() > 0;
    }

    public IEnumerable<Todo> GetAll()
    {
      return _context.Todoes
        .Include(t => t.Category)
        .ToList();
    }

    public IEnumerable<Todo> GetAll(string userId)
    {
      return _context.Todoes
        .Include(t => t.Category)
        .Where(t => t.UserId == userId)
        .ToList();
    }

    public IEnumerable<Todo> GetAll(string userId, string categoryName)
    {
      return _context.Todoes
        .Include(t => t.Category)
        .Where(t => t.UserId == userId && t.Category.Description == categoryName)
        .ToList();
    }

    public Todo GetTodo(int id)
    {
      return _context.Todoes
        .Include(t => t.Category)
        .SingleOrDefault(t => t.Id == id);
    }

    public Todo GetTodo(int id, string userId)
    {
      return _context.Todoes
        .Include(t => t.Category)
        .SingleOrDefault(t => t.Id == id && t.UserId == userId);
    }

    public IEnumerable<Todo> GetAllByCategoryId(int id)
    {
      throw new System.NotImplementedException();
    }
  }
}
