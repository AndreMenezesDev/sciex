using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class RelatorioAnalisadorDuesController : ApiController
	{
		private readonly IRelatorioAnalisadorDuesBll _relatorioAnalisadorDues;

		public RelatorioAnalisadorDuesController(IRelatorioAnalisadorDuesBll relatorioAnalisadorDues)
		{
			_relatorioAnalisadorDues = relatorioAnalisadorDues;
		}

		public List<RelatoriosAnalisadorListaDuesVM> Get([FromUri] RelatorioAnalisadorDuesVM vm)
		{
			return _relatorioAnalisadorDues.GetInfoRelatorio(vm);
		}

	}
}