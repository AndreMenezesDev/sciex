using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class ViewTipoProdEmpSufDropDownController : ApiController
	{
		private readonly IViewProdEmpSufBll _viewProdEmpSuf;

		public ViewTipoProdEmpSufDropDownController(IViewProdEmpSufBll viewProdEmpSuf)
		{
			_viewProdEmpSuf = viewProdEmpSuf;
		}

		public IEnumerable<object> Get([FromUri]ViewProdEmpSufVM vm)
		{
			return _viewProdEmpSuf.ListarChaveTipoProduto(vm);
		}
	}
}