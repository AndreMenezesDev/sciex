using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace Suframa.Sciex.ApplicationService.Services
{
	public class SolicitarInclusaoInsumoController : ApiController
	{
		ISolicitarInclusaoInsumoBll _solicitarInsumoBll;
		public SolicitarInclusaoInsumoController(ISolicitarInclusaoInsumoBll solicitarInsumoBll)
		{
			_solicitarInsumoBll = solicitarInsumoBll;
		}

		public PagedItems<LEInsumoVM> Get([FromUri] LEInsumoVM objeto) =>		
			 _solicitarInsumoBll.ListarPaginado(objeto);

		public string Post([FromBody] List<LEInsumoVM> objeto) =>
			_solicitarInsumoBll.Salvar(objeto);
		
	}
}