using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IPliProcessoAnuenteBll
	{
		IEnumerable<PliProcessoAnuenteVM> Listar(PliProcessoAnuenteVM pliProcessoAnuenteVM);

		PagedItems<PliProcessoAnuenteVM> ListarPaginado(PliProcessoAnuenteVM pagedFilter);

		void Salvar(PliProcessoAnuenteVM pliProcessoAnuenteVM);

		PliProcessoAnuenteVM Selecionar(int? idPliProcessoAnuente);

		void Deletar(int id);

		IEnumerable<object> Listar();
	}
}