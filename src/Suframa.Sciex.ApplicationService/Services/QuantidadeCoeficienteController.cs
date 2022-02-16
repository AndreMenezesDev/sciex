using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class QuantidadeCoeficienteController : ApiController
	{
		private readonly IQuantidadeCoeficienteBll _quantidadeCoeficienteBll;

		public QuantidadeCoeficienteController(IQuantidadeCoeficienteBll quantidadeCoeficienteBll)
		{
			_quantidadeCoeficienteBll = quantidadeCoeficienteBll;
		}
		
		public QuantidadeCoefTecnicoVM Post([FromBody] SolicitacoesAlteracaoVM objeto) =>
				_quantidadeCoeficienteBll.Calcular(objeto);

		public SolicitacoesAlteracaoVM Get([FromUri] SolicitacoesAlteracaoVM objeto) =>
				_quantidadeCoeficienteBll.BucarInfo(objeto);

		public int Put([FromBody] SolicitacoesAlteracaoVM objeto) =>
				_quantidadeCoeficienteBll.Salvar(objeto);



	}
}