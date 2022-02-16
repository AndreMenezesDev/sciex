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
    public class ConsultarLEInsumoController : ApiController
    {

		private readonly ITransferirSaldoInsumoBll _leiInsumo;

		public ConsultarLEInsumoController(ITransferirSaldoInsumoBll leiInsumo)
		{
			_leiInsumo = leiInsumo;
		}

        // GET: api/ConsultarLEInsumo/5
        public LEInsumoVM Get([FromUri] LEInsumoVM obj)
        {
			return _leiInsumo.PesquisarLEInsumo(obj);
		}

        // POST: api/ConsultarLEInsumo
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ConsultarLEInsumo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ConsultarLEInsumo/5
        public void Delete(int id)
        {
        }
    }
}
