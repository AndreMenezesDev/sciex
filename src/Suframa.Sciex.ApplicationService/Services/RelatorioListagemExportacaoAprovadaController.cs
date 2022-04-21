using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.ApplicationService.Services
{
    public class RelatorioListagemExportacaoAprovadaController : ApiController
	{
		private readonly IRelatorioListagemExportacaoBll _bll;

		public RelatorioListagemExportacaoAprovadaController(IRelatorioListagemExportacaoBll relatorioListagemExportacao)
		{
			_bll = relatorioListagemExportacao;
		}

		public LEProdutoComplementoVM Get([FromUri] LEProdutoVM filtro)
		{
			return _bll.ListarRelatorioListagemExportacao(filtro);
		}
	}
}