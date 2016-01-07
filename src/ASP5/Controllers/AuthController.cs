using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP5.Models;
using ASP5.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;

namespace ASP5.Controllers
{
    public class AuthController : Controller
    {
	    private readonly SignInManager<WorldUser> _singInManager;
		public AuthController(SignInManager<WorldUser> singInManager)
		{
			_singInManager = singInManager;
		}
		public IActionResult Login()
	    {
		    if (User.Identity.IsAuthenticated)
		    {
			    return RedirectToAction("Trips", "App");
		    }
			return View();
	    }
		[HttpPost]
	    public async Task<IActionResult> Login(LoginViewModel vm,string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return HttpBadRequest();
			}
			var signInResult = await _singInManager.PasswordSignInAsync(vm.Username, vm.Password, true, false);
			if (signInResult.Succeeded)
			{
				if(string.IsNullOrWhiteSpace(returnUrl))
					return RedirectToAction("Trips", "App");
				return Redirect(returnUrl);
			}
			else
			{
				ModelState.AddModelError("","User Name or Password incorrect");
			}
			return View();
		}
		public async Task<IActionResult> Logout()
		{
			if (User.Identity.IsAuthenticated)
			{
				await _singInManager.SignOutAsync();
			}

			return RedirectToAction("Index", "App");
		}
    }
}
