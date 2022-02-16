using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class NcmDropDownController : ApiController
	{
		private readonly INcmBll _ncmBll;

		public NcmDropDownController(INcmBll ncmBll)
		{
			_ncmBll = ncmBll;
		}

		public IEnumerable<object> Get([FromUri]NcmVM ncmVM)
		{
			return _ncmBll.ListarChave(ncmVM);
		}
	}
}