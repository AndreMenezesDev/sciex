using Suframa.Sciex.BusinessLogic.DeferirPlanoExportacao;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
    public class DeferirPlanoExportacaoController : ApiController
    {
		private readonly IDeferirPlanoExportacaoBll _deferir;

		public DeferirPlanoExportacaoController(IDeferirPlanoExportacaoBll deferir)
		{
			_deferir = deferir;
		}

		// POST: api/DeferirPlanoExportacao
		public ResultadoProcessamentoVM Post([FromBody] PlanoExportacaoVM vm)
        {
			return _deferir.DeferirPlano(vm);
        }
    }
}
