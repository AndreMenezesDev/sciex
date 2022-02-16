using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IMoedaBll
	{
		IEnumerable<object> ListarChave(MoedaVM moedaVM);

		PagedItems<MoedaVM> ListarPaginado(MoedaVM pagedFilter);

		void Salvar(MoedaVM moedaVM);

		MoedaVM Selecionar(int? idMoeda);

		void Deletar(int id);

	}
}