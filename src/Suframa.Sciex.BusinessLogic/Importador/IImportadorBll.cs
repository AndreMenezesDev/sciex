using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IImportadorBll
	{
		IEnumerable<ImportadorVM> Listar(ImportadorVM importadorVM);

		PagedItems<ImportadorVM> ListarPaginado(ImportadorVM pagedFilter);

		void Salvar(ImportadorVM importadorVM);

		ImportadorVM Selecionar(int? idImportador);

		void Deletar(int id);

		IEnumerable<object> Listar();
	}
}