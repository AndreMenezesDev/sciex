using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IFornecedorBll
	{
		IEnumerable<FornecedorVM> Listar(FornecedorVM FornecedorVM);

		PagedItems<FornecedorVM> ListarPaginado(FornecedorVM pagedFilter);

		FornecedorVM Salvar(FornecedorVM fornecedorVM);

		FornecedorVM Selecionar(int? idFornecedor);

		void Deletar(int id);

		IEnumerable<object> ListarChave(FornecedorVM fornecedorVM);
	}
}