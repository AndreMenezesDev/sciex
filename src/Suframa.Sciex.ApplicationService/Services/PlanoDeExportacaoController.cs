using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class PlanoDeExportacaoController : ApiController
	{
		private readonly IAnalisePlanoDeExportacaoBll _planoExportacao;

		public PlanoDeExportacaoController(IAnalisePlanoDeExportacaoBll planoExportacao)
		{
			_planoExportacao = planoExportacao;
		}

		public PagedItems<PlanoExportacaoVM> Get([FromUri] PlanoExportacaoVM objeto)
		{
			return _planoExportacao.ListarPaginado(objeto);
		}

	}
}