using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ASP5.Models
{
	public class WorldUser : IdentityUser
	{
		public DateTime FirstTrip { get; set; }
	}
}