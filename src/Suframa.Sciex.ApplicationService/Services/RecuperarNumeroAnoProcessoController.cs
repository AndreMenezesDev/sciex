using Suframa.Sciex.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class RecuperarNumeroAnoProcessoController : ApiController
	{
		IMinhaSolicitacaoAlteracaoBll _bll;
		public RecuperarNumeroAnoProcessoController(IMinhaSolicitacaoAlteracaoBll bll)
		{
			_bll = bll;
		}

		public string Get(int id) => _bll.BuscarNumeroAnoProcessoPorIdProcesso(id);
	}
}