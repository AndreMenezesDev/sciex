using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class NcmExcecaoController : ApiController
	{
		private readonly INcmExcecaoBll _ncmExcecaoBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="ncmExcecaoBll"></param>
		public NcmExcecaoController(INcmExcecaoBll ncmExcecaoBll)
		{
			_ncmExcecaoBll = ncmExcecaoBll;
		}

		/// <summary>Seleciona a NcmExcecao pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public NcmExcecaoVM Get(int id)
		{
			return _ncmExcecaoBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de NcmExcecao</summary>
		/// <param name="ncmExcecaoVM"></param>
		/// <returns></returns>
		public IEnumerable<NcmExcecaoVM> Get([FromUri]NcmExcecaoVM ncmExcecaoVM)
		{
			return _ncmExcecaoBll.Listar(ncmExcecaoVM);
		}

		/// <summary>Salva a NcmExcecao</summary>
		/// <param name="ncmExcecaoVM"></param>
		/// <returns></returns>
		public NcmExcecaoVM Put([FromBody]NcmExcecaoVM ncmExcecaoVM)
		{
			return _ncmExcecaoBll.Salvar(ncmExcecaoVM);
		}

		/// <summary>Deletar NcmExcecao pelo ID</summary>
		/// <param name="id">ID NcmExcecao</param>
		public void Delete(int id)
		{
			_ncmExcecaoBll.Deletar(id);
		}
	}
}