using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApPliProdutocationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PliProdutoController : ApiController
	{
		private readonly IPliProdutoBll _PliProdutoBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="PliProdutoBll"></param>
		public PliProdutoController(IPliProdutoBll PliProdutoBll)
		{
			_PliProdutoBll = PliProdutoBll;
		}
		
		/// <summary>Seleciona a PliProduto pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PliProdutoVM Get(int id)
		{
			return _PliProdutoBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de PliProduto</summary>
		/// <param name="PliProdutoVM"></param>
		/// <returns></returns>
		public IEnumerable<PliProdutoVM> Get([FromUri]PliProdutoVM PliProdutoVM)
		{
			return _PliProdutoBll.Listar(PliProdutoVM);
		}

		/// <summary>Valida a regra de cadastro do PliProduto</summary>
		/// <param name="PliProdutoVM"></param>
		/// <returns></returns>
		public PliProdutoVM Put([FromBody]PliProdutoVM PliProdutoVM)
		{
			_PliProdutoBll.Salvar(PliProdutoVM);
			return PliProdutoVM;
		}


		/// <summary>Deletar PliProduto pelo ID</summary>
		/// <param name="id">ID PliProduto</param>
		public void Delete(int id)
		{
			_PliProdutoBll.Deletar(id);
		}
	}
}