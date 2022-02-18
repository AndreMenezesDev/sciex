using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
    public class AprovarTodosDUEEmAnaliseController : ApiController
    {
		private readonly IDueBll _bll;

		public AprovarTodosDUEEmAnaliseController(IDueBll bll)
		{
			_bll = bll;
		}


        // POST: api/AprovarTodosInsumosEDetalhesAnalise
		[HttpPost]
        public ResultadoMensagemProcessamentoVM AprovarTodosInsumosEDetalhes([FromBody]PlanoExportacaoDUEComplementoVM vm)
        {
			return _bll.AprovarTodasDeclaracoesUnicasExportacao(vm);
        }

    }
}
