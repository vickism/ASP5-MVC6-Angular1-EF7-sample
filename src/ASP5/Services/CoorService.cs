using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ASP5.Services
{
	public class CoorService
    {
	    private readonly ILogger<CoorService> _logger;

	    public CoorService(ILogger<CoorService> logger )
	    {
		    _logger = logger;
	    }

	    public async Task <CoorServiceResult> Lookup(string location)
	    {
		    var result = new CoorServiceResult()
		    {
			    Success = false,
			    Message = "Failed looking up coordinates"
		    };
		    var encodedName = WebUtility.UrlEncode(location);
		    var bingKey = Startup.Configuration["AppSettings:BingKey"];
			
			var url = $"http://dev.virtualearth.net/REST/v1/Locations?q={encodedName}&key={bingKey}";
		    var client = new HttpClient();
		    var json = await client.GetStringAsync(url);
			var results = JObject.Parse(json);

			var resources = results["resourceSets"][0]["resources"];
		    if (!resources.HasValues)
		    {
			    result.Message = $"Could not find '{location}' as a location";
			    return result;
		    }
		    var confidence = (string) resources[0]["confidence"];
		    if (confidence != "High")
		    {
			    result.Message = $"Could not find a confident match for '{location}' as a location";
		    }
		    else
		    {
			    var coords = resources[0]["geocodePoints"][0]["coordinates"];
			    result.Latitude = (double) coords[0];
			    result.Longitude = (double) coords[1];
			    result.Success = true;
			    result.Message = "Success";
		    }

		    return result;
	    }
    }
}
