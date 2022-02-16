using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IConsultarEntradaDIBll
	{
		PagedItems<DIArquivoEntradaVM> ListarPaginado(ParametrosDIEntradaVM pagedFilter);
	}
}
