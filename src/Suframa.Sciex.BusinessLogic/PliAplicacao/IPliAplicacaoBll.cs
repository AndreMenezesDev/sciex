using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IPliAplicacaoBll
	{
		IEnumerable<PliAplicacaoVM> Listar(PliAplicacaoVM pliAplicacaoVM);

		PagedItems<PliAplicacaoVM> ListarPaginado(PliAplicacaoVM pagedFilter);

		void Salvar(PliAplicacaoVM pliAplicacaoVM);

		PliAplicacaoVM Selecionar(int? idPliAplicacao);

		void Deletar(int id);

		IEnumerable<object> Listar();

		IEnumerable<object> ListarSemTodos();
	}
}