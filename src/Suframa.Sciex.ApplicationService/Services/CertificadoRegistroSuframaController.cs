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
    public class CertificadoRegistroSuframaController : ApiController
    {
		private readonly ICertificadoRegistroSuframaBll _bll ;

		public CertificadoRegistroSuframaController(ICertificadoRegistroSuframaBll bll)
		{
			_bll = bll;
		}

        // GET: api/CertificadoRegistro
        public CertificadoRegistroVM Get(int id)
        {
			return _bll.CarregarDadosCertificado(id);
        }
    }
}
