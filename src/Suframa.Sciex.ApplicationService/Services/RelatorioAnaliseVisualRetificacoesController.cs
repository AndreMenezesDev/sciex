using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class RelatorioAnaliseVisualRetificacoesController : ApiController
	{
		private readonly IPliAnaliseVisualBll _pliAnaliseVisualBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="pliBll"></param>
		public RelatorioAnaliseVisualRetificacoesController(IPliAnaliseVisualBll pliAnaliseVisualBll)
		{
			_pliAnaliseVisualBll = pliAnaliseVisualBll;
		}

		/// <summary>Seleciona a Pli pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IEnumerable<RelatorioRetificacoesVM> Get(int id)
		{
			return _pliAnaliseVisualBll.ListarRelatorio(id);
		}
	}
}