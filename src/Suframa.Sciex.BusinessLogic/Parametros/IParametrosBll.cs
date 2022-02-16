using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IParametrosBll
	{
		IEnumerable<ParametrosVM> Listar(ParametrosVM parametrosVM);
		IEnumerable<object> ListarChave(ParametrosVM parametrosVM);

		PagedItems<ParametrosVM> ListarPaginado(ParametrosVM pagedFilter);

		void Salvar(ParametrosVM parametrosVM);

		ParametrosVM Selecionar(int? idParametros);

		void Deletar(int id);
	}
}