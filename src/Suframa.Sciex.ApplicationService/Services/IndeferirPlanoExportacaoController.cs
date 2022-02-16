using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class IndeferirPlanoExportacaoController : ApiController
	{
		private readonly IAnalisePlanoDeExportacaoBll _bll;

		public IndeferirPlanoExportacaoController(IAnalisePlanoDeExportacaoBll bll)
		{
			_bll = bll;
		}
		
		//public DadosDetalhesInsumosVM Get(int id)
		//{
		//	return _bll.ListarDetalhePorIdInsumo(id);
		//}

		public AnalisarPlanoExportacaoVM Post([FromBody] AnalisarPlanoExportacaoVM vm)
		{
			return _bll.IndeferirPlano(vm);
		}

	}
}