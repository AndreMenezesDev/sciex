using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class ViewProdutoEmpresaDropDownController : ApiController
	{
		private readonly IViewProdutoEmpresaBll _viewProdutoEmpresa;

		public ViewProdutoEmpresaDropDownController(IViewProdutoEmpresaBll viewProdutoEmpresa)
		{
			_viewProdutoEmpresa = viewProdutoEmpresa;
		}

		public IEnumerable<object> Get([FromUri]ViewProdutoEmpresaVM viewProdutoEmpresaVM)
		{
			return _viewProdutoEmpresa.ListarChave(viewProdutoEmpresaVM);
		}
	}
}