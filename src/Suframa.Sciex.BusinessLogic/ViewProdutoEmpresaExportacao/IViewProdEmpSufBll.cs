using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IViewProdEmpSufBll
	{
		IEnumerable<object> ListarChaveProduto(ViewProdEmpSufVM vw);
		IEnumerable<object> ListarChaveTipoProduto(ViewProdEmpSufVM vw);
		IEnumerable<object> ListarChaveNCM(ViewProdEmpSufVM vm);
		IEnumerable<object> ListarChaveUnidadeMedida(ViewProdEmpSufVM vm);
		ViewProdEmpSufVM Selecionar(string desc);
	}
}