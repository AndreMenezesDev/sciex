using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;


namespace Suframa.Sciex.ApplicationService.Services
{
	public class SolicitacoesAlteracaoDetalheController : ApiController
	{
		IMinhaSolicitacaoAlteracaoDetalheBll _minhaSolicitacaoDetalhe;
		public SolicitacoesAlteracaoDetalheController(IMinhaSolicitacaoAlteracaoDetalheBll minhaSolicitacaoDetalhe)
		{
			_minhaSolicitacaoDetalhe = minhaSolicitacaoDetalhe;
		}

		public PagedItems<PRCSolicDetalheVM> Get([FromUri] PRCSolicDetalheVM objeto) =>
				_minhaSolicitacaoDetalhe.ListarPaginado(objeto);

		public DetalhesMinhaSolicitacaoAlteracaoVM Get(int Id) =>
			_minhaSolicitacaoDetalhe.BuscarInfoDetalhes(Id);

		public int Delete(int id) =>
			_minhaSolicitacaoDetalhe.ApagarDetalheSolicitacaoAlteracao(id);
		
	}
}