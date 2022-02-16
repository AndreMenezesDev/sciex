using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class LiController : ApiController
	{
		private readonly ILiBll _liBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="liBll"></param>
		public LiController(ILiBll liBll)
		{
			_liBll = liBll;
		}
		
		/// <summary>Seleciona a Li pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public LiVM Get(int id)
		{
			return _liBll.Selecionar(id);
		}

		/// <summary>Seleciona a Li pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public LiVM Get(long idMercadoria)
		{
			return _liBll.SelecionarPorMercadoria(idMercadoria);
		}

		/// <summary>Seleciona uma lista de Li</summary>
		/// <param name="liVM"></param>
		/// <returns></returns>
		public IEnumerable<LiVM> Get([FromUri]LiVM liVM)
		{
			return _liBll.Listar();
		}

		/// <summary>Salva a Li</summary>
		/// <param name="liVM"></param>
		/// <returns></returns>
		public LiVM Put([FromBody]LiVM liVM)
		{
			_liBll.Salvar(liVM);
			return liVM;
		}

		/// <summary>Deletar Li pelo ID</summary>
		/// <param name="id">ID Li</param>
		public void Delete(int id)
		{
			_liBll.Deletar(id);
		}
	}
}