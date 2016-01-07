using System.Collections.Generic;

namespace ASP5.Models
{
	public interface IWorldRepository
	{
		IEnumerable<Trip> GetAllTrips();
		IEnumerable<Trip> GetAllTripsWithStops();
		void AddTrip(Trip trip);
		bool SaveAll();
		Trip GetTripByName(string tripName, string username);
		void addStop(string tripName, Stop newStop, string userName);
		IEnumerable<Trip> GetUserTripsWithStops(string name);
	}
}