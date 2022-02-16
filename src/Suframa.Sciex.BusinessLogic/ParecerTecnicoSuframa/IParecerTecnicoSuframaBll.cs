using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IParecerTecnicoSuframaBll
	{
		PagedItems<ParecerTecnicoVM> ListarPaginado(ParecerTecnicoVM pagedFilter);
		ParecerTecnicoVM Selecionar(int id);
		RelatorioParecerTecnicoVM SelecionarRelatorio(int id);
	}
}