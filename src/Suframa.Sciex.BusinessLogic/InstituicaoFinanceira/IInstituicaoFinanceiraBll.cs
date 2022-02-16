using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IInstituicaoFinanceiraBll
	{
		IEnumerable<InstituicaoFinanceiraVM> Listar(InstituicaoFinanceiraVM instituicaoFinanceiraVM);

		PagedItems<InstituicaoFinanceiraVM> ListarPaginado(InstituicaoFinanceiraVM pagedFilter);

		void Salvar(InstituicaoFinanceiraVM instituicaoFinanceiraVM);

		InstituicaoFinanceiraVM Selecionar(int? idInstituicaoFinanceira);

		void Deletar(int id);

		IEnumerable<object> ListarChave(InstituicaoFinanceiraVM instituicaoFinanceiraVM);
	}
}