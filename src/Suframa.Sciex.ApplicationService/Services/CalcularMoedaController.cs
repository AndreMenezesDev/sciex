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
    public class CalcularMoedaController : ApiController
    {
		private readonly ICalcularMoedaBll _calcularMoedaBll;

		public CalcularMoedaController(ICalcularMoedaBll calcularMoedaBll)
		{
			_calcularMoedaBll = calcularMoedaBll;
		}

        // GET: api/CacularMoeda
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CacularMoeda/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CacularMoeda
        public CalcularMoedaVM Post([FromBody] SolicitacoesAlteracaoVM objeto)
        {
			return _calcularMoedaBll.CalcularMoeda(objeto);
        }

        // PUT: api/CacularMoeda/5
        public bool Put([FromBody] SolicitacoesAlteracaoVM objeto)
		{
			return _calcularMoedaBll.Salvar(objeto);
        }

        // DELETE: api/CacularMoeda/5
        public void Delete(int id)
        {
        }
    }
}
