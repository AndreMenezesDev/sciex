using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ITipoDeclaracaoBll
	{
		IEnumerable<TipoDeclaracaoVM> Listar(TipoDeclaracaoVM codigoContaVM);

		IEnumerable<object> ListarCodigoConta(TipoDeclaracaoVM codigoContaVM);

		PagedItems<TipoDeclaracaoVM> ListarPaginado(TipoDeclaracaoVM pagedFilter);

		void Salvar(TipoDeclaracaoVM codigoContaVM);

		TipoDeclaracaoVM Selecionar(int? idCodigoConta);

		void Deletar(int id);

		IEnumerable<object> ListarChave(TipoDeclaracaoVM codigoContaVM);
	}
}