using System;
using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class SolicAlteracaoPaisController : ApiController
	{
		private readonly ISolicAlteracaoPaisBll _solicitacaoPaiBll;
		public SolicAlteracaoPaisController(ISolicAlteracaoPaisBll solicitacaoPaiBll)
		{
			_solicitacaoPaiBll = solicitacaoPaiBll;
		}

		public SolicitacoesAlteracaoVM Get([FromUri] SolicitacoesAlteracaoVM objeto) => _solicitacaoPaiBll.Buscar(objeto);

		public int Post([FromBody] SolicitacoesAlteracaoVM objeto) => _solicitacaoPaiBll.Salvar(objeto);
				

	}
}