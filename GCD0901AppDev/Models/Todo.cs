using GCD0901AppDev.Enums;

using Microsoft.AspNetCore.Identity;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCD0901AppDev.Models
{
  public class Todo
  {
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "You need to add Description ...")]
    [StringLength(255)]
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public TodoStatus Status { get; set; } = TodoStatus.Todo;
    [Required]
    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
  }
}
