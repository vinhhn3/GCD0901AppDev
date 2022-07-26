﻿using GCD0901AppDev.Data;
using GCD0901AppDev.Models;
using GCD0901AppDev.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GCD0901AppDev.Controllers
{
  [Authorize(Roles = Role.ADMIN)]
  public class AdminController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly ApplicationDbContext _context;

    public AdminController(
      UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
      _userManager = userManager;
      _context = context;
    }

    public IActionResult Index()
    {
      return View();
    }
    [HttpGet]
    public IActionResult Users()
    {
      var usersWithPermission = _userManager.GetUsersInRoleAsync(Role.USER).Result;
      return View(usersWithPermission);
    }
  }
}
