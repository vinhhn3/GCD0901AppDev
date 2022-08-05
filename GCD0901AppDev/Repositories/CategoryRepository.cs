using GCD0901AppDev.Data;
using GCD0901AppDev.Models;
using GCD0901AppDev.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

namespace GCD0901AppDev.Repositories
{
  public class CategoryRepository : ICategoryRepository
  {
    private ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public IEnumerable<Category> GetAll()
    {
      return _context.Categories.ToList();
    }

    public Category GetById(int id)
    {
      return _context.Categories
        .Include(t => t.Todoes)
        .SingleOrDefault(t => t.Id == id);
    }
  }
}
