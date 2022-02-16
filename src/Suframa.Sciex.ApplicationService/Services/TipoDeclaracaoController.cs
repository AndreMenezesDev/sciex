using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class TipoDeclaracaoController : ApiController
	{
		private readonly ITipoDeclaracaoBll _tipoDeclaracaoBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="codigoContaBll"></param>
		public TipoDeclaracaoController(ITipoDeclaracaoBll tipoDeclaracaoBll)
		{
			_tipoDeclaracaoBll = tipoDeclaracaoBll;
		}

		/// <summary>Seleciona a CodigoConta pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public TipoDeclaracaoVM Get(int id)
		{
			return _tipoDeclaracaoBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de CodigoConta</summary>
		/// <param name="codigoContaVM"></param>
		/// <returns></returns>
		public IEnumerable<TipoDeclaracaoVM> Get([FromUri]TipoDeclaracaoVM tipoDeclaracaoVM)
		{
			return _tipoDeclaracaoBll.Listar(tipoDeclaracaoVM);
		}

		/// <summary>Salva a CodigoConta</summary>
		/// <param name="codigoContaVM"></param>
		/// <returns></returns>
		public TipoDeclaracaoVM Put([FromBody]TipoDeclaracaoVM tipoDeclaracaoVM)
		{
			_tipoDeclaracaoBll.Salvar(tipoDeclaracaoVM);
			return tipoDeclaracaoVM;
		}

		/// <summary>Deletar CodigoConta pelo ID</summary>
		/// <param name="id">ID CodigoConta</param>
		public void Delete(int id)
		{
			_tipoDeclaracaoBll.Deletar(id);
		}
	}
}