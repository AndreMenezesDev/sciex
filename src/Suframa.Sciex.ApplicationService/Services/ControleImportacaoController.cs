using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ControleImportacaoController : ApiController
	{
		private readonly IControleImportacaoBll _controleImportacaoBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="controleImportacaoBll"></param>
		public ControleImportacaoController(IControleImportacaoBll controleImportacaoBll)
		{
			_controleImportacaoBll = controleImportacaoBll;
		}

		/// <summary>Seleciona a ControleImportacao pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ControleImportacaoVM Get(int id)
		{
			return _controleImportacaoBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de ControleImportacao</summary>
		/// <param name="controleImportacaoVM"></param>
		/// <returns></returns>
		public IEnumerable<ControleImportacaoVM> Get([FromUri]ControleImportacaoVM controleImportacaoVM)
		{
			return _controleImportacaoBll.Listar(controleImportacaoVM);
		}

		/// <summary>Salva a ControleImportacao</summary>
		/// <param name="controleImportacaoVM"></param>
		/// <returns></returns>
		public ControleImportacaoVM Put([FromBody]ControleImportacaoVM controleImportacaoVM)
		{
			_controleImportacaoBll.Salvar(controleImportacaoVM);
			return controleImportacaoVM;
		}

		/// <summary>Deletar ControleImportacao pelo ID</summary>
		/// <param name="id">ID ControleImportacao</param>
		public void Delete(int id)
		{
			_controleImportacaoBll.Deletar(id);
		}
	}
}