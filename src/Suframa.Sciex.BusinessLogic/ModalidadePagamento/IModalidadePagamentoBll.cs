using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IModalidadePagamentoBll
	{
		IEnumerable<ModalidadePagamentoVM> Listar(ModalidadePagamentoVM modalidadePagamentoVM);

		PagedItems<ModalidadePagamentoVM> ListarPaginado(ModalidadePagamentoVM pagedFilter);

		void Salvar(ModalidadePagamentoVM modalidadePagamentoVM);

		ModalidadePagamentoVM Selecionar(int? idModalidadePagamento);

		void Deletar(int id);

		IEnumerable<object> ListarChave(ModalidadePagamentoVM modalidadePagamentoVM);
	}
}