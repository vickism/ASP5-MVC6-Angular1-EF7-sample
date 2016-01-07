using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP5.ViewModels
{
    public class ContactViewModal
    {
		[Required]
		[StringLength(255,MinimumLength = 5)]
	    public string Name { get; set; }
		[Required]
		[EmailAddress]
	    public string Email { get; set; }
		[Required]
		[StringLength(1024,MinimumLength = 5)]
	    public string Message { get; set; }
    }
}
