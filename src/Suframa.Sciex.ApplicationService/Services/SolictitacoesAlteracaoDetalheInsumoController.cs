using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class SolictitacoesAlteracaoDetalheInsumoController : ApiController
	{
		ISolicitacaoAlteracaoBll _solicitacaoAlteracaoBll;
		public SolictitacoesAlteracaoDetalheInsumoController(ISolicitacaoAlteracaoBll solicitacaoAlteracaoBll)
		{
			_solicitacaoAlteracaoBll = solicitacaoAlteracaoBll;
		}

		public List<PRCSolicDetalheVM> Get([FromUri] SolicitacoesAlteracaoVM objeto) => _solicitacaoAlteracaoBll.BuscarDados(objeto);				
		
					
	}
}