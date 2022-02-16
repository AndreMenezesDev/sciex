using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ICodigoContaBll
	{
		IEnumerable<CodigoContaVM> Listar(CodigoContaVM codigoContaVM);

		IEnumerable<object> ListarCodigoConta(CodigoContaVM codigoContaVM);

		PagedItems<CodigoContaVM> ListarPaginado(CodigoContaVM pagedFilter);

		void Salvar(CodigoContaVM codigoContaVM);

		CodigoContaVM Selecionar(int? idCodigoConta);

		void Deletar(int id);

		IEnumerable<object> ListarChave(CodigoContaVM codigoContaVM);
	}
}