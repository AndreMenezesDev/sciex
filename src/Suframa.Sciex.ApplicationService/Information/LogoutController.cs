using DotNetCasClient;
using Suframa.Sciex.CrossCutting.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Information
{
	public class LogoutController : ApiController
	{

		[AllowAnonymous]
		public HttpResponseMessage Get()
		{
			if (!PrivateSettings.DEVELOPMENT_MODE)
			{
				CasAuthentication.SingleSignOut();
			}

			return Request.CreateResponse(HttpStatusCode.OK);
		}
	}
}