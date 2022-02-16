using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PliAnaliseVisualController : ApiController
	{
		private readonly IPliAnaliseVisualBll _pliAnaliseVisualBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="pliBll"></param>
		public PliAnaliseVisualController(IPliAnaliseVisualBll pliAnaliseVisualBll)
		{
			_pliAnaliseVisualBll = pliAnaliseVisualBll;
		}
		
		/// <summary>Seleciona a Pli pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PliVM Get(int id)
		{
			return _pliAnaliseVisualBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de Pli</summary>
		/// <param name="pliVM"></param>
		/// <returns></returns>
		public IEnumerable<PliVM> Get([FromUri]PliVM pliVM)
		{
			return _pliAnaliseVisualBll.Listar(pliVM);
		}

		/// <summary>Valida a regra de cadastro do PLI</summary>
		/// <param name="pliVM"></param>
		/// <returns></returns>
		public PliVM Put([FromBody]PliVM pliVM)
		{
			pliVM = _pliAnaliseVisualBll.Salvar(pliVM);
			return pliVM;
		}


		/// <summary>Deletar Pli pelo ID</summary>
		/// <param name="id">ID Pli</param>
		public void Delete(int id)
		{
			_pliAnaliseVisualBll.Deletar(id);
		}
	}
}