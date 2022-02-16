using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class CertificadoController : ApiController
	{
		private readonly ICertificadoRegistroBll _certificado;

		public CertificadoController(ICertificadoRegistroBll certificado)
		{
			_certificado = certificado;
		}

		public PagedItems<PRCStatusVM> Get([FromUri] PRCStatusVM vm)
		{
			return _certificado.ListarPaginado(vm);
		}

	}
}