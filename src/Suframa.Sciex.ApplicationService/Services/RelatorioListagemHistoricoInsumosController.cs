using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class RelatorioListagemHistoricoInsumosController : ApiController
	{
		IRelatorioListagemHistoricoInsumosBll _bll;
		public RelatorioListagemHistoricoInsumosController(IRelatorioListagemHistoricoInsumosBll bll)
		{
			_bll = bll;
		}

		public List<RelatorioListagemHistoricoInsumosVM> Post([FromBody] RelatorioListagemHistoricoInsumosVM filterVm) =>
			_bll.BuscarRelatorio(filterVm);

	}
}