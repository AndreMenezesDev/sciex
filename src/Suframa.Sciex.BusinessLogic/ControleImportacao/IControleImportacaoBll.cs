using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IControleImportacaoBll
	{
		IEnumerable<ControleImportacaoVM> Listar(ControleImportacaoVM controleImportacaoVM);

		PagedItems<ControleImportacaoVM> ListarPaginado(ControleImportacaoVM pagedFilter);

		void Salvar(ControleImportacaoVM controleImportacaoVM);

		ControleImportacaoVM Selecionar(int? idControleImportacao);

		void Deletar(int id);

		IEnumerable<object> ListarSetor();
	}
}