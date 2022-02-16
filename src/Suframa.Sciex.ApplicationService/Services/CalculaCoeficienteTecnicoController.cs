using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class CalculaCoeficienteTecnicoController : ApiController
	{
		private readonly IQuantidadeCoeficienteBll _quantidadeCoeficienteBll;

		public CalculaCoeficienteTecnicoController(IQuantidadeCoeficienteBll quantidadeCoeficienteBll)
		{
			_quantidadeCoeficienteBll = quantidadeCoeficienteBll;
		}

		public decimal Post([FromBody] SolicitacoesAlteracaoVM objeto) =>
				_quantidadeCoeficienteBll.CalculaQuantidadeMaxima(objeto);

	}
}