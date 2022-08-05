using GCD0901AppDev.Models;

using System.Collections.Generic;

namespace GCD0901AppDev.Repositories.Interfaces
{
  public interface ICategoryRepository
  {
    Category GetById(int id);
    IEnumerable<Category> GetAll();
  }
}
