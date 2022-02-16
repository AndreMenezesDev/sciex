using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class CodigoUtilizacaoController : ApiController
	{
		private readonly ICodigoUtilizacaoBll _codigoUtilizacaoBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="codigoUtilizacaoBll"></param>
		public CodigoUtilizacaoController(ICodigoUtilizacaoBll codigoUtilizacaoBll)
		{
			_codigoUtilizacaoBll = codigoUtilizacaoBll;
		}

		/// <summary>Seleciona a CodigoUtilizacao pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public CodigoUtilizacaoVM Get(int id)
		{
			return _codigoUtilizacaoBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de CodigoUtilizacao</summary>
		/// <param name="codigoUtilizacaoVM"></param>
		/// <returns></returns>
		public IEnumerable<CodigoUtilizacaoVM> Get([FromUri]CodigoUtilizacaoVM codigoUtilizacaoVM)
		{
			return _codigoUtilizacaoBll.Listar(codigoUtilizacaoVM);
		}

		/// <summary>Salva a CodigoUtilizacao</summary>
		/// <param name="codigoUtilizacaoVM"></param>
		/// <returns></returns>
		public CodigoUtilizacaoVM Put([FromBody]CodigoUtilizacaoVM codigoUtilizacaoVM)
		{
			_codigoUtilizacaoBll.Salvar(codigoUtilizacaoVM);
			return codigoUtilizacaoVM;
		}

		/// <summary>Deletar CodigoUtilizacao pelo ID</summary>
		/// <param name="id">ID CodigoUtilizacao</param>
		public void Delete(int id)
		{
			_codigoUtilizacaoBll.Deletar(id);
		}
	}
}