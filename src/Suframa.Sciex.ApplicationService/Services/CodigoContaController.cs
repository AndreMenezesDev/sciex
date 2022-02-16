using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class CodigoContaController : ApiController
	{
		private readonly ICodigoContaBll _codigoContaBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="codigoContaBll"></param>
		public CodigoContaController(ICodigoContaBll codigoContaBll)
		{
			_codigoContaBll = codigoContaBll;
		}

		/// <summary>Seleciona a CodigoConta pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public CodigoContaVM Get(int id)
		{
			return _codigoContaBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de CodigoConta</summary>
		/// <param name="codigoContaVM"></param>
		/// <returns></returns>
		public IEnumerable<CodigoContaVM> Get([FromUri]CodigoContaVM codigoContaVM)
		{
			return _codigoContaBll.Listar(codigoContaVM);
		}

		/// <summary>Salva a CodigoConta</summary>
		/// <param name="codigoContaVM"></param>
		/// <returns></returns>
		public CodigoContaVM Put([FromBody]CodigoContaVM codigoContaVM)
		{
			_codigoContaBll.Salvar(codigoContaVM);
			return codigoContaVM;
		}

		/// <summary>Deletar CodigoConta pelo ID</summary>
		/// <param name="id">ID CodigoConta</param>
		public void Delete(int id)
		{
			_codigoContaBll.Deletar(id);
		}
	}
}