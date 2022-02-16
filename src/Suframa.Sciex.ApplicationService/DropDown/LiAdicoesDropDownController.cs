using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class LiAdicoesDropDownController : ApiController
	{
		private readonly IDiBll _diBll;

		public LiAdicoesDropDownController(IDiBll diBll)
		{
			_diBll = diBll;
		}

		public IEnumerable<object> Get([FromUri]DiVM diVm)
		{
			return _diBll.ListarLiAdicoes(diVm);
		}
	}
}