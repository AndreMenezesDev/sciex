using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface INaladiBll
	{
		IEnumerable<NaladiVM> Listar(NaladiVM naladiVM);

		PagedItems<NaladiVM> ListarPaginado(NaladiVM pagedFilter);

		void Salvar(NaladiVM naladiVM);

		NaladiVM Selecionar(int? idNaladi);

		void Deletar(int id);

		IEnumerable<object> ListarChave(NaladiVM naladiVM);
	}
}