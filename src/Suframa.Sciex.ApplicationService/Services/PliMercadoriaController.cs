using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApPliMercadoriacationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PliMercadoriaController : ApiController
	{
		private readonly IPliMercadoriaBll _PliMercadoriaBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="PliMercadoriaBll"></param>
		public PliMercadoriaController(IPliMercadoriaBll PliMercadoriaBll)
		{
			_PliMercadoriaBll = PliMercadoriaBll;
		}
		
		/// <summary>Seleciona a PliMercadoria pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PliMercadoriaVM Get(int id)
		{
			return _PliMercadoriaBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de PliMercadoria</summary>
		/// <param name="PliMercadoriaVM"></param>
		/// <returns></returns>
		public IEnumerable<PliMercadoriaVM> Get([FromUri]PliMercadoriaVM PliMercadoriaVM)
		{
			return _PliMercadoriaBll.Listar(PliMercadoriaVM);
		}

		/// <summary>Valida a regra de cadastro do PliMercadoria</summary>
		/// <param name="PliMercadoriaVM"></param>
		/// <returns></returns>
		public PliMercadoriaVM Put([FromBody]PliMercadoriaVM PliMercadoriaVM)
		{
			PliMercadoriaVM = _PliMercadoriaBll.Salvar(PliMercadoriaVM);
			return PliMercadoriaVM;
		}

		/// <summary>Deletar PliMercadoria pelo ID</summary>
		/// <param name="id">ID PliMercadoria</param>
		public void Delete(int id)
		{
			_PliMercadoriaBll.Deletar(id);
		}
	}
}