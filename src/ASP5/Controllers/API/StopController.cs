using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASP5.Models;
using ASP5.Services;
using ASP5.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;

namespace ASP5.Controllers.API
{
	[Route("api/trips/{tripName}/stops")]
	[Authorize]
	public class StopController : Controller
    {
	    private readonly IWorldRepository _repository;
	    private readonly ILogger _logger;
		private readonly CoorService _coorService;

		public StopController(IWorldRepository repository, ILogger<StopController> logger, CoorService coorService)
	    {
		    _repository = repository;
		    _logger = logger;
			_coorService = coorService;
	    }
		[HttpGet()]
		public JsonResult Get(string tripName)
		{
			try
			{
				var result = _repository.GetTripByName(tripName,User.Identity.Name);
				if (result == null)
				{
					return Json(null);
				}
				return Json(AutoMapper.Mapper.Map<IEnumerable<StopViewModel>>(result.Stops.OrderBy(s=> s.Order)));
			}
			catch (Exception e)
			{
				_logger.LogError($"Failed to get stops for trip {tripName}",e);
				Response.StatusCode = (int) HttpStatusCode.BadRequest;
				return Json(e);
			}
		}
		[HttpPost()]
		public async Task<JsonResult> Post(string tripName, [FromBody] StopViewModel vm)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					Response.StatusCode = (int)HttpStatusCode.BadRequest;
					return Json("Validation Failed");
				}
				var newStop = AutoMapper.Mapper.Map<Stop>(vm);
				var coordResults = await _coorService.Lookup(newStop.Name);
				if (!coordResults.Success)
				{
					Response.StatusCode = (int)HttpStatusCode.Created;
					return Json(coordResults.Message);
				}
				newStop.Latitude = coordResults.Latitude;
				newStop.Longitude= coordResults.Longitude;
				
				_repository.addStop(tripName,newStop, User.Identity.Name);
				if (_repository.SaveAll())
				{
					Response.StatusCode = (int) HttpStatusCode.Created;
					return Json(Mapper.Map<StopViewModel>(newStop));
				}
				return Json("Failed to save file");
			}
			catch (Exception e)
			{
				_logger.LogError("Failed to save stop code", e);
				Response.StatusCode = (int) HttpStatusCode.BadRequest;
				return Json(e);
			}
		}
    }
}