using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IConsultarEntradaDIProcessadoBll
	{
		PagedItems<DIEntradaVM> ListarPaginado(DIEntradaVM pagedFilter);
	}
}
