using GCD0901AppDev.Data;
using GCD0901AppDev.Models;
using GCD0901AppDev.Repositories.Interfaces;
using GCD0901AppDev.ViewModels;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

namespace GCD0901AppDev.Repositories
{
  public class TodoRepository : ITodoRepository
  {
    private ApplicationDbContext _context;
    public TodoRepository(ApplicationDbContext context)
    {
      _context = context;
    }
    public bool CreateTodo(TodoCategoriesViewModel viewModel)
    {
      throw new System.NotImplementedException();
    }

    public bool DeleteById(int id)
    {
      throw new System.NotImplementedException();
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
      throw new System.NotImplementedException();
    }

    public IEnumerable<Todo> GetTodoesByCategoryId(int id)
    {
      throw new System.NotImplementedException();
    }
  }
}
