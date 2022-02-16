using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
    public class CertificadoRegistroController : ApiController
    {
		private readonly ICertificadoRegistroBll _certificado ;

		public CertificadoRegistroController(ICertificadoRegistroBll certificado)
		{
			_certificado = certificado;
		}

        // GET: api/CertificadoRegistro
        public CertificadoRegistroVM Get(int id)
        {
			return _certificado.CarregarDadosCertificado(id);
        }
    }
}
