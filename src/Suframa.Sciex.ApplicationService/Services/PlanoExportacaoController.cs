using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PlanoExportacaoController : ApiController
	{
		private readonly IPlanoExportacaoBll _planoExportacaoBll;

		public PlanoExportacaoController(IPlanoExportacaoBll planoExportacaoBll)
		{
			_planoExportacaoBll = planoExportacaoBll;
		}

		public PlanoExportacaoVM Get(int id)
		{
			return _planoExportacaoBll.Selecionar(id);
		}

		public PagedItems<PlanoExportacaoVM> Get([FromUri] ConsultarPlanoExportacaoVM vm)
		{
			return _planoExportacaoBll.ListarPaginado(vm);
		}

		public NovoPlanoExportacaoVM Post([FromBody] NovoPlanoExportacaoVM vm)
		{
			return _planoExportacaoBll.SalvarNovoPlano(vm);
		}

		public bool Put([FromBody] PlanoExportacaoVM vm)
		{
			return _planoExportacaoBll.CopiarPlano(vm);
		}

		public bool Delete(int id)
		{
			return _planoExportacaoBll.DeletarPlano(id);
		}
	}
}

