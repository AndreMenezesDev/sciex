using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class BuscarUltimoInsumoAnteriorPorIdInsumoAtualController : ApiController
	{
		private readonly IPEInsumoBll _bll;

		public BuscarUltimoInsumoAnteriorPorIdInsumoAtualController(IPEInsumoBll bll)
		{
			_bll = bll;
		}

		public PEInsumoVM Get(int id)
		{
			return _bll.SelecionarInsumoAnteriorPorIdInsumoAtual(id);
		}

		//public PagedItems<PEDetalheInsumoImportadoVM> Get([FromUri] SalvarDetalheVM vm)
		//{
		//	return _bll.SelecionarInsumoAnteriorPorIdInsumoAtual(vm);
		//}

		//public LEInsumoVM Put([FromBody]LEInsumoVM vm)
		//{
		//	vm = _bll.Salvar(vm);
		//	return vm;
		//}

		//public void Delete(int id)
		//{
		//	_bll.Deletar(id);
		//}
	}
}