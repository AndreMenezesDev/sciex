using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class AprovarAnaliseLEProdutoInsumoController : ApiController
	{
		private readonly ILEAnaliseProdutoBll _leAnaliseProdutoBll;

		public AprovarAnaliseLEProdutoInsumoController(ILEAnaliseProdutoBll leAnaliseProdutoBll)
		{
			_leAnaliseProdutoBll = leAnaliseProdutoBll;
		}

		public LEProdutoVM Put([FromBody] LEProdutoVM vm)
		{
			vm = _leAnaliseProdutoBll.AprovarAnalise(vm);
			return vm;
		}
	}
}