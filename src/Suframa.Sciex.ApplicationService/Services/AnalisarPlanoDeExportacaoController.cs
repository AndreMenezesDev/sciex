using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class AnalisarPlanoDeExportacaoController : ApiController
	{
		private readonly IAnalisePlanoDeExportacaoBll _planoExportacao;

		public AnalisarPlanoDeExportacaoController(IAnalisePlanoDeExportacaoBll planoExportacao)
		{
			_planoExportacao = planoExportacao;
		}

		public AnalisarPlanoExportacaoVM Post([FromBody] AnalisarPlanoExportacaoVM objeto)
		{
			return _planoExportacao.SalvarAnalise(objeto);
		}

	}
}