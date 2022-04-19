using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class RelatorioErrorDuesController : ApiController
	{
		private readonly IRelatorioErrosDuesBll _bll;

		public RelatorioErrorDuesController(IRelatorioErrosDuesBll bll)
		{
			_bll = bll;
		}
		
		public List<RelatorioErrosDuesVM> Post([FromBody] RelatorioErrosDuesVM filterVm) => 
			_bll.GerarRelatorio(filterVm);

	}
}