using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IAladiBll
	{
		IEnumerable<AladiVM> Listar(AladiVM aladiVM);

		PagedItems<AladiVM> ListarPaginado(AladiVM pagedFilter);

		void Salvar(AladiVM aladiVM);

		AladiVM Selecionar(int? idAladi);

		void Deletar(int id);

		IEnumerable<object> ListarChave(AladiVM aladiVM);
	}
}