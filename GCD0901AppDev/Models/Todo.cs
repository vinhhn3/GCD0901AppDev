using GCD0901AppDev.Enums;

using System;

namespace GCD0901AppDev.Models
{
  public class Todo
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public TodoStatus Status { get; set; } = TodoStatus.Todo;


  }
}
