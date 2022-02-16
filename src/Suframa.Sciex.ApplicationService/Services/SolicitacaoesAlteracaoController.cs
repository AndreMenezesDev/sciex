using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;


namespace Suframa.Sciex.ApplicationService.Services
{
	public class SolicitacaoesAlteracaoController : ApiController
	{
		IMinhaSolicitacaoAlteracaoBll _minhaSolicitacao;
		public SolicitacaoesAlteracaoController(IMinhaSolicitacaoAlteracaoBll minhaSolicitacao)
		{
			_minhaSolicitacao = minhaSolicitacao;
		}

		public PagedItems<PRCSolicitacaoAlteracaoVM> Get([FromUri] PRCSolicitacaoAlteracaoVM objeto) =>	
				_minhaSolicitacao.ListarPaginado(objeto);
		

	}
}