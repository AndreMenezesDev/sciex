using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class RegimeTributarioMercadoriaController : ApiController
	{
		private readonly IRegimeTributarioMercadoriaBll _regimeTributarioMercadoriaBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="regimeTributarioMercadoriaBll"></param>
		public RegimeTributarioMercadoriaController(IRegimeTributarioMercadoriaBll regimeTributarioMercadoriaBll)
		{
			_regimeTributarioMercadoriaBll = regimeTributarioMercadoriaBll;
		}

		/// <summary>Seleciona a RegimeTributarioMercadoria pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public RegimeTributarioMercadoriaVM Get(int id)
		{
			return _regimeTributarioMercadoriaBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de RegimeTributarioMercadoria</summary>
		/// <param name="regimeTributarioMercadoriaVM"></param>
		/// <returns></returns>
		public IEnumerable<RegimeTributarioMercadoriaVM> Get([FromUri]RegimeTributarioMercadoriaVM regimeTributarioMercadoriaVM)
		{
			return _regimeTributarioMercadoriaBll.Listar(regimeTributarioMercadoriaVM);
		}

		/// <summary>Salva a RegimeTributarioMercadoria</summary>
		/// <param name="regimeTributarioMercadoriaVM"></param>
		/// <returns></returns>
		public RegimeTributarioMercadoriaVM Put([FromBody]RegimeTributarioMercadoriaVM regimeTributarioMercadoriaVM)
		{
			_regimeTributarioMercadoriaBll.Salvar(regimeTributarioMercadoriaVM);
			return regimeTributarioMercadoriaVM;
		}

		/// <summary>Deletar RegimeTributarioMercadoria pelo ID</summary>
		/// <param name="id">ID RegimeTributarioMercadoria</param>
		public void Delete(int id)
		{
			_regimeTributarioMercadoriaBll.Deletar(id);
		}
	}
}