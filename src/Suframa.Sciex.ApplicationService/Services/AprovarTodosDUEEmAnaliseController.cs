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
    public class AprovarTodosInsumosEDetalhesAnaliseController : ApiController
    {
		private readonly IPEInsumoBll _bll;

		public AprovarTodosInsumosEDetalhesAnaliseController(IPEInsumoBll bll)
		{
			_bll = bll;
		}


		// GET: api/AprovarTodosInsumosEDetalhesAnalise
		public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AprovarTodosInsumosEDetalhesAnalise/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/AprovarTodosInsumosEDetalhesAnalise
		[HttpPost]
        public ResultadoMensagemProcessamentoVM AprovarTodosInsumosEDetalhes([FromBody] ListarInsumosNacionalImportadosVM vm)
        {
			return _bll.AprovarTodosInsumosEDetalhes(vm);
        }

        // PUT: api/AprovarTodosInsumosEDetalhesAnalise/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/AprovarTodosInsumosEDetalhesAnalise/5
        public void Delete(int id)
        {
        }
    }
}
