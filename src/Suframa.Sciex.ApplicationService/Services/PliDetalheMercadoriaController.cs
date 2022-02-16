using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApPliDetalheMercadoriacationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PliDetalheMercadoriaController : ApiController
	{
		private readonly IPliDetalheMercadoriaBll _PliDetalheMercadoriaBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="PliDetalheMercadoriaBll"></param>
		public PliDetalheMercadoriaController(IPliDetalheMercadoriaBll PliDetalheMercadoriaBll)
		{
			_PliDetalheMercadoriaBll = PliDetalheMercadoriaBll;
		}
		
		/// <summary>Seleciona a PliDetalheMercadoria pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PliDetalheMercadoriaVM Get(int id)
		{
			return _PliDetalheMercadoriaBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de PliDetalheMercadoria</summary>
		/// <param name="PliDetalheMercadoriaVM"></param>
		/// <returns></returns>
		public IEnumerable<PliDetalheMercadoriaVM> Get([FromUri]PliDetalheMercadoriaVM PliDetalheMercadoriaVM)
		{
			return _PliDetalheMercadoriaBll.Listar(PliDetalheMercadoriaVM);
		}

		/// <summary>Valida a regra de cadastro do PliDetalheMercadoria</summary>
		/// <param name="PliDetalheMercadoriaVM"></param>
		/// <returns></returns>
		public PliDetalheMercadoriaVM Put([FromBody]PliDetalheMercadoriaVM PliDetalheMercadoriaVM)
		{
			_PliDetalheMercadoriaBll.Salvar(PliDetalheMercadoriaVM);
			return PliDetalheMercadoriaVM;
		}


		/// <summary>Deletar PliDetalheMercadoria pelo ID</summary>
		/// <param name="id">ID PliDetalheMercadoria</param>
		public void Delete(int id)
		{
			_PliDetalheMercadoriaBll.Deletar(id);
		}
	}
}