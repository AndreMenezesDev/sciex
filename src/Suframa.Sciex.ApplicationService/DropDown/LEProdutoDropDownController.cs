using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class LEProdutoDropDownController : ApiController
	{
		private readonly ILEProdutoBll _bll;

		public LEProdutoDropDownController(ILEProdutoBll bll)
		{
			_bll = bll;
		}

		public IEnumerable<object> Get([FromUri]LEProdutoVM vw)
		{
			return _bll.ListarChave(vw);
		}
	}
}