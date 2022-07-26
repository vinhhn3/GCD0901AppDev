using GCD0901AppDev.Models;

using System.Collections.Generic;

namespace GCD0901AppDev.ViewModels
{
  public class TodoCategoriesViewModel
  {
    public Todo Todo { get; set; }
    public IEnumerable<Category> Categories { get; set; }
  }
}
