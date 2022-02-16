using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IViewImportadorBll
	{
		IEnumerable<ViewImportadorVM> Listar(ViewImportadorVM viewImportadorVM);
		IEnumerable<object> ListarChave(ViewImportadorVM viewImportadorVM);
		PagedItems<ViewImportadorVM> ListarPaginado(ViewImportadorVM pagedFilter);
		ViewImportadorVM Selecionar(string cnpj);
		ViewImportadorVM SelecionarInscricao(int inscricao);
	}
}