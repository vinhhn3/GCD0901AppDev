using GCD0901AppDev.Models;

using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCD0901AppDev.ViewModels
{
  public class TodoCategoriesViewModel
  {
    public Todo Todo { get; set; }
    public IEnumerable<Category> Categories { get; set; }
    [Display(Name = "File")]
    public IFormFile FormFile { get; set; }
  }
}
