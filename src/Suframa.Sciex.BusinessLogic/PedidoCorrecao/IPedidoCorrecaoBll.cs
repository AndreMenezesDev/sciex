using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IPedidoCorrecaoBll
	{
		void Apagar(int? id);

		IEnumerable<ResumoVM> ListarItensAtualizar(ProtocoloVM protocoloVM);

		IEnumerable<ResumoVM> ListarItensCorrecao(ProtocoloVM protocoloVM);

		IEnumerable<ResumoVM> ListarItensCorrigidos(ProtocoloVM protocoloVM);

		IEnumerable<ResumoVM> ListarItensCorrigir(ProtocoloVM protocoloVM);

		void RegrasSalvar(PedidoCorrecaoEntity pedidoCorrecao, bool IsChamadaInterna = false);

		void Salvar(PedidoCorrecaoVM[] pedidoCorrecaoVM);

		PedidoCorrecaoVM Selecionar(int id);
	}
}