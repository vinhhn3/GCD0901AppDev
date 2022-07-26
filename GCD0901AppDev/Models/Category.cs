using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCD0901AppDev.Models
{
  public class Category
  {
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Description cannot be null ...")]
    [StringLength(255)]
    public string Description { get; set; }
    public List<Todo> Todoes { get; set; }
  }
}
