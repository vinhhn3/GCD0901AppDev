using GCD0901AppDev.DTOs.Requests;
using GCD0901AppDev.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace GCD0901AppDev.Repositories.Interfaces
{
  public interface ITodoRepository
  {
    IEnumerable<Todo> GetAll();
    IEnumerable<Todo> GetAll(string userId);
    IEnumerable<Todo> GetAll(string userId, string categoryName);
    IEnumerable<Todo> GetAllByCategoryId(int id);

    Task<bool> CreateTodo(CreateTodoRequest model, string userId);
    bool DeleteTodo(int id, string userId);
    Todo GetTodo(int id);
    Todo GetTodo(int id, string userId);
    bool EditTodo(EditTodoRequest model, string userId);
  }
}
