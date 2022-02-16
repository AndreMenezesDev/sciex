using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class SolicitacaoAlteracaoValorFreteController : ApiController
	{
		private readonly IValorFreteBll _bll;

		public SolicitacaoAlteracaoValorFreteController(IValorFreteBll bll)
		{
			_bll = bll;
		}

		public SolicitacoesAlteracaoVM Get([FromUri]SolicitacoesAlteracaoVM dados)
		{
			return _bll.BuscarInfo(dados);
		}

		public ValorFreteVM Post([FromBody] SolicitacoesAlteracaoVM objeto)
		{
			return _bll.Calcular(objeto);
		}

		public int Put([FromBody] SolicitacoesAlteracaoVM objeto) =>
				_bll.Salvar(objeto);

		//public bool Delete(int id)
		//{
		//	return _bll.ExcluirSolicitacao(id); ;
		//}


	}
}