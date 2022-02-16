using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class DetalheInsumosController : ApiController
	{
		IDetalheInsumosBll _detalheInsumosBll;
		public DetalheInsumosController(IDetalheInsumosBll detalheInsumosBll)
		{
			_detalheInsumosBll = detalheInsumosBll;
		}
		public bool Post([FromBody] PRCInsumoVM objeto) =>
			_detalheInsumosBll.Deletar(objeto);

		public int Put([FromBody] SalvarDetalhePRCInsumoVM objeto) =>
			_detalheInsumosBll.SalvarNovoDetalhe(objeto);

		public bool Delete(int id) =>
			_detalheInsumosBll.Deletar(id);

	}
}