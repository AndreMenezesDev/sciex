using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IUnidadeReceitaFederalBll
	{
		IEnumerable<UnidadeReceitaFederalVM> Listar(UnidadeReceitaFederalVM unidadeReceitaFederalVM);

		IEnumerable<object> ListarChave(UnidadeReceitaFederalVM unidadeReceitaFederalVM);


		PagedItems<UnidadeReceitaFederalVM> ListarPaginado(UnidadeReceitaFederalVM pagedFilter);

		void Salvar(UnidadeReceitaFederalVM unidadeReceitaFederalVM);

		UnidadeReceitaFederalVM Selecionar(int? idUnidadeReceitaFederal);

		void Deletar(int id);
	}
}