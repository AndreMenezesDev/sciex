using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface INcmExcecaoBll
	{
		IEnumerable<NcmExcecaoVM> Listar(NcmExcecaoVM ncmExcecaoVM);

		PagedItems<NcmExcecaoVM> ListarPaginado(NcmExcecaoVM pagedFilter);

		NcmExcecaoVM Salvar(NcmExcecaoVM ncmExcecaoVM);

		NcmExcecaoVM Selecionar(int? idNcmExcecao);

		void Deletar(int id);

		IEnumerable<object> ListarChave(NcmExcecaoVM ncmExcecaoVM);

		IEnumerable<object> ListarChave(ViewNcmVM viewNcmVM);
	}
}