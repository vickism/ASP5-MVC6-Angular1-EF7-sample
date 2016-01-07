using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP5.Models;
using ASP5.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP5.Controllers.API
{
    [Route("api/[controller]")]
	[Authorize]
    public class TripController : Controller
    {
	    private readonly IWorldRepository _repository;
	    private readonly ILogger<TripController> _logger;

	    public TripController(IWorldRepository repository, ILogger<TripController> logger )
	    {
		    _repository = repository;
		    _logger = logger;
	    }

	    // GET: api/values
        [HttpGet]
        public object Get()
        {
	        var trips = _repository.GetUserTripsWithStops(User.Identity.Name);
			var result  = Mapper.Map<IEnumerable<TripViewModel>>(trips);
	        return result;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]TripViewModel vm)
        {
	        try
	        {
		        if (!ModelState.IsValid) return HttpBadRequest(vm);

		        var newTrip = Mapper.Map<Trip>(vm);
		        newTrip.UserName = User.Identity.Name;

				_repository.AddTrip(newTrip);
		        if (!_repository.SaveAll()) return HttpBadRequest();
		        _logger.LogInformation("Saving to database");
		        return Created("", Mapper.Map<TripViewModel>(newTrip));
	        }
	        catch (Exception e)
	        {
				_logger.LogError("Failed to save new trips",e);
		        return HttpBadRequest(e);
	        }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
