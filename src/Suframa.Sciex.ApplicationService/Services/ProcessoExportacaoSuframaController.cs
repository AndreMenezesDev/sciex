using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ProcessoExportacaoSuframaController : ApiController
	{
		private readonly IProcessoExportacaoSuframaBll _bll;

		public ProcessoExportacaoSuframaController(IProcessoExportacaoSuframaBll bll)
		{
			_bll = bll;
		}

		public ProcessoExportacaoVM Get(int id)
		{
			return _bll.Selecionar(id);
		}

		public PagedItems<ProcessoExportacaoVM> Get([FromUri] ConsultarProcessoExportacaoVM vm)
		{
			return _bll.ListarPaginado(vm);
		}

	}
}