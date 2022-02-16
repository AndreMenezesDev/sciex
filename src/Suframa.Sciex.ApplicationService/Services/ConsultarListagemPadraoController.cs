using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ConsultarListagemPadraoController : ApiController
	{
		private readonly IViewDetalheMercadoriaBll _bll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="ConsultarPliBll"></param>
		public ConsultarListagemPadraoController(IViewDetalheMercadoriaBll bll)
		{
			_bll = bll;
		}

		public PagedItems<ViewDetalheMercadoriaVM> Get([FromUri]ViewDetalheMercadoriaVM detalheMercadoriaVM)
		{
			return _bll.ListagemPadrao(detalheMercadoriaVM);
		}

	}
}