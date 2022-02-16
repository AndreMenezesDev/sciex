using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IFabricanteBll
	{
		IEnumerable<FabricanteVM> Listar(FabricanteVM fabricanteVM);

		IEnumerable<object> ListarChave(FabricanteVM fabricanteVM);

		PagedItems<FabricanteVM> ListarPaginado(FabricanteVM pagedFilter);

		FabricanteVM Salvar(FabricanteVM fabricanteVM);

		FabricanteVM Selecionar(int? idFabricante);

		FabricanteVM Visualizar(FabricanteVM fabricanteVM);

		void Deletar(int id);
	}
}