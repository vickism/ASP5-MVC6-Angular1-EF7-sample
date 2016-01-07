using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;

namespace ASP5.Models
{
	public class WorldRepository : IWorldRepository
	{
		private readonly WorldContext _context;
		private readonly ILogger<WorldRepository> _logger;

		public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
		{
			_context = context;
			_logger = logger;
		}

		public IEnumerable<Trip> GetAllTrips()
		{
			try
			{
				return _context.Trips.OrderBy(t => t.Name);
			}
			catch (Exception ex)
			{
				_logger.LogError("Could not get trips from database", ex);
				return null;
			}
		}

		public IEnumerable<Trip> GetAllTripsWithStops()
		{
			try
			{
				return _context.Trips.Include(t => t.Stops)
					.OrderBy(t => t.Name);
			}
			catch (Exception ex)
			{
				_logger.LogError("Could not get trips from database", ex);
				return null;
			}
		}

		public void AddTrip(Trip trip)
		{
			_context.Trips.Add(trip);
		}

		public bool SaveAll()
		{
			return _context.SaveChanges() > 0;
		}

		public Trip GetTripByName(string tripName,string username)
		{
			return _context.Trips
				.Include(t => t.Stops)
				.FirstOrDefault(t => t.Name == tripName && t.UserName == username);
		}

		public void addStop(string tripName, Stop newStop, string userName)
		{
			var theTrip = GetTripByName(tripName,userName);
			newStop.Order = theTrip.Stops.Max(s => s.Order) + 1;
			theTrip.Stops.Add(newStop);
			_context.Stops.Add(newStop);
		}

		public IEnumerable<Trip> GetUserTripsWithStops(string name)
		{
			try
			{
				return _context.Trips
					.Include(t => t.Stops)
					.OrderBy(t => t.Name)
					.Where(t => t.UserName == name);
			}
			catch (Exception ex)
			{
				_logger.LogError("Could not get trips from database", ex);
				return null;
			}
		}
	}
}
