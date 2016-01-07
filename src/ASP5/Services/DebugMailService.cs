using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASP5.Services
{
    public class DebugMailService : IMailService
    {
	    public bool SendMail(string to, string from, string subject, string body)
	    {
			Debug.WriteLine($"Sending email to {to} with body: {body}");
		    return true;
	    }
    }
}
