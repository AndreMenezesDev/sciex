using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IViewRelatorioAnaliseProcessamentoPli
	{
		PagedItems<ViewRelatorioAnaliseProcessamentoPliVM> ListarPaginado(ViewRelatorioAnaliseProcessamentoPliVM pagedFilter);
	}
}