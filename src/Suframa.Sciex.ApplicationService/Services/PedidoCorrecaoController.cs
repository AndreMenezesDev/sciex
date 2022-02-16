using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Pedido Correção service</summary>
	public class PedidoCorrecaoController : ApiController
	{
		private readonly IPedidoCorrecaoBll _pedidoCorrecaoBll;

		/// <summary>Pedido Correção injetar regras de negócio</summary>
		/// <param name="pedidoCorrecaoBll"></param>
		public PedidoCorrecaoController(IPedidoCorrecaoBll pedidoCorrecaoBll)
		{
			_pedidoCorrecaoBll = pedidoCorrecaoBll;
		}

		/// <summary>Excluir pedido correção</summary>
		/// <param name="id">ID do pedido correção</param>
		public void Delete(int? id)
		{
			_pedidoCorrecaoBll.Apagar(id);
		}

		/// <summary>Salva pedido correção</summary>
		/// <param name="pedidoCorrecao">ID do pedido correção</param>
		public void Put(PedidoCorrecaoVM[] pedidoCorrecao)
		{
			_pedidoCorrecaoBll.Salvar(pedidoCorrecao);
		}
	}
}