using GCD0901AppDev.Models;

using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCD0901AppDev.DTOs.Requests
{
  public class CreateTodoRequest
  {
    [Required(ErrorMessage = "You need to add Description ...")]
    [StringLength(255)]
    public string Description { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public IEnumerable<Category> Categories { get; set; }
    [Display(Name = "File")]
    public IFormFile FormFile { get; set; }
  }
}
