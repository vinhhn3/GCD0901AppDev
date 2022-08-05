using GCD0901AppDev.Models;
using GCD0901AppDev.ViewModels;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace GCD0901AppDev.Repositories.Interfaces
{
  public interface ITodoRepository
  {
    IEnumerable<Todo> GetAll();
    Task<bool> CreateTodoWithUserId(TodoCategoriesViewModel viewModel, string userId);
    bool DeleteByIdAndUserId(int id, string userId);
    Todo GetById(int id);
    Todo GetByTodoIdAndUserId(int id, string userId);
    bool EditTodo(TodoCategoriesViewModel viewModel, string userId);
    IEnumerable<Todo> GetTodoesByCategoryId(int id);
  }
}
