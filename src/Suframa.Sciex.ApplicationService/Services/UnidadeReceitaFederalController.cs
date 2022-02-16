using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class UnidadeReceitaFederalController : ApiController
	{
		private readonly IUnidadeReceitaFederalBll _unidadeReceitaFederalBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="unidadeReceitaFederalBll"></param>
		public UnidadeReceitaFederalController(IUnidadeReceitaFederalBll unidadeReceitaFederalBll)
		{
			_unidadeReceitaFederalBll = unidadeReceitaFederalBll;
		}
		
		/// <summary>Seleciona a UnidadeReceitaFederal pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public UnidadeReceitaFederalVM Get(int id)
		{
			return _unidadeReceitaFederalBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de UnidadeReceitaFederal</summary>
		/// <param name="unidadeReceitaFederalVM"></param>
		/// <returns></returns>
		public IEnumerable<UnidadeReceitaFederalVM> Get([FromUri]UnidadeReceitaFederalVM unidadeReceitaFederalVM)
		{
			return _unidadeReceitaFederalBll.Listar(unidadeReceitaFederalVM);
		}

		/// <summary>Salva a UnidadeReceitaFederal</summary>
		/// <param name="unidadeReceitaFederalVM"></param>
		/// <returns></returns>
		public UnidadeReceitaFederalVM Put([FromBody]UnidadeReceitaFederalVM unidadeReceitaFederalVM)
		{
			_unidadeReceitaFederalBll.Salvar(unidadeReceitaFederalVM);
			return unidadeReceitaFederalVM;
		}

		/// <summary>Deletar UnidadeReceitaFederal pelo ID</summary>
		/// <param name="id">ID UnidadeReceitaFederal</param>
		public void Delete(int id)
		{
			_unidadeReceitaFederalBll.Deletar(id);
		}
	}
}