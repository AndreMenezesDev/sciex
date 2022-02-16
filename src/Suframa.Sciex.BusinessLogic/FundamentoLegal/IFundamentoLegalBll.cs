using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IFundamentoLegalBll
	{
		PagedItems<FundamentoLegalVM> ListarPaginado(FundamentoLegalPagedFilterVM pagedFilter);

		void Salvar(FundamentoLegalVM fundamentoLegalVM);

		FundamentoLegalVM Selecionar(int? idFundamentoLegal);

		FundamentoLegalVM Visualizar(FundamentoLegalVM fundamentoLegalVM);

		void Deletar(int id);

		IEnumerable<object> ListarChave(FundamentoLegalVM fundamentoLegalVM);
	}
}