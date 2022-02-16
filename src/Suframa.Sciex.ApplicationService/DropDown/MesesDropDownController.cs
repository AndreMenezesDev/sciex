using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown de meses</summary>
	public class MesesDropDownController : ApiController
	{
		/// <summary>Obtem ID e descrição dos meses</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IEnumerable<object> Get()
		{
			return new List<object>
			{
				new { key = 1, value = "Janeiro" },
				new { key = 2, value = "Fevereiro" },
				new { key = 3, value = "Março" },
				new { key = 4, value = "Abril" },
				new { key = 5, value = "Maio" },
				new { key = 6, value = "Junho" },
				new { key = 7, value = "Julho" },
				new { key = 8, value = "Agosto" },
				new { key = 9, value = "Setembro" },
				new { key = 10, value = "Outubro" },
				new { key = 11, value = "Novembro" },
				new { key = 12, value = "Dezembro" },
			};
		}
	}
}