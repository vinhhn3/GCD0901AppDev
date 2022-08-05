using GCD0901AppDev.Models;
using GCD0901AppDev.ViewModels;

using System.Collections.Generic;

namespace GCD0901AppDev.Repositories.Interfaces
{
  public interface ITodoRepository
  {
    IEnumerable<Todo> GetAll();
    bool CreateTodo(TodoCategoriesViewModel viewModel);
    bool DeleteById(int id);
    Todo GetById(int id);
    bool EditTodo(TodoCategoriesViewModel viewModel);
    IEnumerable<Todo> GetTodoesByCategoryId(int id);
  }
}
