using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IRegimeTributarioTesteBll
	{
		PagedItems<RegimeTributarioTesteVM> ListarPaginado(RegimeTributarioTestePagedFilterVM pagedFilter);

		void Salvar(RegimeTributarioTesteVM regimeTributarioVM);

		RegimeTributarioTesteVM Selecionar(int? idRegimeTributario);

		RegimeTributarioTesteVM Visualizar(RegimeTributarioTesteVM regimeTributarioTesteVM);

		void Deletar(int id);
	}
}