using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service fabricante</summary>
	public class FornecedorController : ApiController
	{
		private readonly IFornecedorBll _fornecedorBll;
		private readonly IPaisBll _paisBll;

		/// <summary>Fabricante injetar regras de negócio</summary>
		/// <param name="fornecedorBll"></param>
		public FornecedorController(IFornecedorBll fornecedorBll, IPaisBll paisBll)
		{
			_fornecedorBll = fornecedorBll;
			_paisBll = paisBll;
		}

		/// <summary>Deletar o fabricante pelo ID</summary>
		/// <param name="id">ID fabricante</param>
		public void Delete(int id)
		{
			_fornecedorBll.Deletar(id);
		}

		/// <summary>Seleciona a Aladi pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public FornecedorVM Get(int id)
		{
			var fornecedor = _fornecedorBll.Selecionar(id);

			if (fornecedor != null)
			{
				fornecedor.DescricaoPais = _paisBll.ListarDescricaoPais(fornecedor.CodigoPais);
			}

			return fornecedor;
		}

		/// <summary>Persistir fabricante</summary>
		/// <param name="fornecedorVM">Objeto fabricante a ser persistido</param>
		/// <returns></returns>
		public FornecedorVM Put([FromBody]FornecedorVM fornecedorVM)
		{
			return _fornecedorBll.Salvar(fornecedorVM);
		}
	}
}