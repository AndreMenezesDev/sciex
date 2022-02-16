using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IViewProdutoEmpresaBll
	{
		IEnumerable<object> ListarChave(ViewProdutoEmpresaVM viewProdutoEmpresaVM);
		PagedItems<ViewProdutoEmpresaVM> ListarPaginado(ViewProdutoEmpresaVM pagedFilter);
		ViewProdutoEmpresaVM Selecionar(string desc);


	}
}