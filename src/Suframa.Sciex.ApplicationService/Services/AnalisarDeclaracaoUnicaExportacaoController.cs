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
    public class AnalisarDeclaracaoUnicaExportacaoController : ApiController
    {
		private IDueBll _bll;

		public AnalisarDeclaracaoUnicaExportacaoController(IDueBll bll)
		{
			_bll = bll;
		}

		// GET: api/DeclaracaoUnicaExportacao/5
		public PagedItems<PlanoExportacaoDUEComplementoVM> Get([FromUri] PEProdutoVM vm)
        {
            return _bll.ListarDUEPaginadoParaAnalise(vm);
        }

		public ResultadoMensagemProcessamentoVM Post([FromBody] AnalisePlanoExportacaoDUEVM vm)
		{
			return _bll.AprovarOuReprovarDeclaracaoUnicaExportacao(vm);
		}
	}
}
