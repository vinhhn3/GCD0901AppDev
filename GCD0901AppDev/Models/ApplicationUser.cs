using Microsoft.AspNetCore.Identity;

using System.Collections.Generic;

namespace GCD0901AppDev.Models
{
  public class ApplicationUser : IdentityUser
  {
    public string FullName { get; set; }
    public string Address { get; set; }
    public List<Todo> Todoes { get; set; }
  }
}
