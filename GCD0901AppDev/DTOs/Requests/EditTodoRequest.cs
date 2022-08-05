using GCD0901AppDev.Enums;
using GCD0901AppDev.Models;

using System.Collections.Generic;

namespace GCD0901AppDev.DTOs.Requests
{
  public class EditTodoRequest
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public TodoStatus Status { get; set; }
    public int CategoryId { get; set; }
    public IEnumerable<Category> Categories { get; set; }
  }
}
