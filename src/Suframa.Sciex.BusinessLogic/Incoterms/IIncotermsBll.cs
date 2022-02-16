using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IIncotermsBll
	{

		IEnumerable<object> ListarChave(IncotermsVM incotermsVM);

		IEnumerable<IncotermsVM> Listar(IncotermsVM incotermsVM);		

		PagedItems<IncotermsVM> ListarPaginado(IncotermsVM pagedFilter);

		void Salvar(IncotermsVM incotermsVM);

		IncotermsVM Selecionar(int? idIncoterms);

		void Deletar(int id);
	}
}