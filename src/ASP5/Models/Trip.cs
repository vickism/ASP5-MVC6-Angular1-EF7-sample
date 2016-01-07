using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP5.Models
{
    public class Trip
    {
	    public string Name { get; set; }
	    public DateTime Created { get; set; }
	    public string UserName { get; set; }
	    public int Id { get; set; }

		public ICollection<Stop> Stops { get; set; }
    }
}
