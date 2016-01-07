using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP5.Models;
using ASP5.Services;
using ASP5.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;

namespace ASP5.Controllers.Web
{
    public class AppController : Controller
    {
	    private readonly IMailService _mailService;
	    private readonly IWorldRepository _repository;


	    public AppController(IMailService mailService,IWorldRepository repository)
	    {
		    _mailService = mailService;
		    _repository = repository;
	    }

	    // GET: /<controller>/
        public IActionResult Index()
        {
			try {
				return View();
			}
			catch (Exception e)
			{
				throw e;
			}
        }

		[Authorize]
	    public IActionResult Trips()
	    {
			return View();
	    }

	    public IActionResult About()
	    {
		    return View();
	    }
		public IActionResult Contact()
		{
			var m = new ContactViewModal();
			return View(m);
		}
		[HttpPost]
	    public IActionResult Contact(ContactViewModal model)
		{
			if (ModelState.IsValid)
			{

				_mailService.SendMail(Startup.Configuration["AppSettings:SiteEmailAddress"], model.Email, $"sub from {model.Name}",
					model.Message);
				ModelState.Clear();
				ViewBag.Message = "Mail Sent Thanks";
			}
			return View();

		}
	}
}
