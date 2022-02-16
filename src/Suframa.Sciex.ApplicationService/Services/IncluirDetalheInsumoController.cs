using FluentValidation;
using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class IncluirDetalheInsumoController : ApiController
	{
		IIncluirDetalheInsumoBll _incluirDetalheInsumoBll;
		public IncluirDetalheInsumoController(IIncluirDetalheInsumoBll incluirDetalheInsumoBll)
		{
			_incluirDetalheInsumoBll = incluirDetalheInsumoBll;
		}

		public bool Post([FromBody] AnalisarPlanoExportacaoVM objeto)
		{
			return true; //_incluirDetalheInsumoBll.SalvarAnalise(objeto);
		}

	}
}