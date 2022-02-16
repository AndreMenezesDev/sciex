using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ProcessoExportacaoController : ApiController
	{
		private readonly IProcessoExportacaoBll _processoExportacaoBll;

		public ProcessoExportacaoController(IProcessoExportacaoBll processoExportacaoBll)
		{
			_processoExportacaoBll = processoExportacaoBll;
		}

		public ProcessoExportacaoVM Get(int id)
		{
			return _processoExportacaoBll.Selecionar(id);
		}

		public PagedItems<ProcessoExportacaoVM> Get([FromUri] ConsultarProcessoExportacaoVM vm)
		{
			return _processoExportacaoBll.ListarPaginado(vm);
		}

	}
}