using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface INcmBll
	{
		IEnumerable<NcmVM> Listar(NcmVM ncmVM);

		PagedItems<NcmVM> ListarPaginado(NcmVM pagedFilter);

		NcmVM Salvar(NcmVM ncmVM);

		NcmVM Selecionar(int? idNcm);

		void Deletar(int id);

		IEnumerable<object> ListarChave(NcmVM ncmVM);

		IEnumerable<object> ListarChave(ViewNcmVM viewNcmVM);

		NcmVM Selecionar(NcmVM ncmVM);
	}
}