using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ICodigoUtilizacaoBll
	{
		IEnumerable<CodigoUtilizacaoVM> Listar(CodigoUtilizacaoVM codigoUtilizacaoVM);

		PagedItems<CodigoUtilizacaoVM> ListarPaginado(CodigoUtilizacaoVM pagedFilter);

		void Salvar(CodigoUtilizacaoVM codigoUtilizacaoVM);

		CodigoUtilizacaoVM Selecionar(int? idCodigoUtilizacao);

		void Deletar(int id);

		IEnumerable<object> ListarChave(CodigoUtilizacaoVM codigoUtilizacaoVM);

		IEnumerable<object> ListarCodigoUtilizacao(CodigoUtilizacaoVM codigoUtilizacaoVM);
	}
}