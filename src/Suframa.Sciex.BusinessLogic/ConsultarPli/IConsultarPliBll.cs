using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IConsultarPliBll
	{

		PagedItems<PliVM> ListarPaginado(PliVM pagedFilter);

		void Salvar(PliVM ConsultarPliVM);

		PliVM Selecionar(long? idPli);

	}
}