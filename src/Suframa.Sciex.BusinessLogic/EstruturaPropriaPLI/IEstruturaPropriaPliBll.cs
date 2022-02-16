using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IEstruturaPropriaPliBll
	{
		IEnumerable<EstruturaPropriaPLIVM> Listar(EstruturaPropriaPLIVM estruturaPropriaPLIVM);
		PagedItems<EstruturaPropriaPLIVM> ListarPaginado(EstruturaPropriaPLIVM pagedFilter);
		void Salvar(EstruturaPropriaPLIVM estruturaPropriaPLIVM);
		EstruturaPropriaPLIVM Selecionar(int? idEstruturaPropriaPli);
		void Deletar(int id);
		IEnumerable<object> ListarChave(EstruturaPropriaPLIVM estruturaPropriaPLIVM);		
	}
}