using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IErroProcessamentoBll
	{
		IEnumerable<ErroProcessamentoVM> Listar(ErroProcessamentoVM ErroProcessamentoVM);

		PagedItems<ErroProcessamentoVM> ListarPaginado(ErroProcessamentoVM pagedFilter);
		DadosErroProcessamentoVM ListarErrosLePaginado(EstruturaPropriaLEEntityVM pagedFilter);
		DadosErroProcessamentoPEVM ListarErrosPEPaginado(EstruturaPropriaPEVM pagedFilter);

	}
}