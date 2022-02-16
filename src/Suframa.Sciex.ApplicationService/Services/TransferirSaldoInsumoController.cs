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
    public class TransferirSaldoInsumoController : ApiController
    {
		private readonly ITransferirSaldoInsumoBll _saldoInsumo;


		public TransferirSaldoInsumoController(ITransferirSaldoInsumoBll saldoInsumo)
		{
			_saldoInsumo = saldoInsumo;
		}

        // GET: api/TransferirSaldoInsumo/5
        public PRCInsumoVM Get(int id)
        {
			return _saldoInsumo.PesquisarPRCInsumo(id);
        }

        // POST: api/TransferirSaldoInsumo
        public bool Post([FromBody] PRCInsumoVM InsumoVM)
        {
			return _saldoInsumo.SalvarPrcInsumo(InsumoVM);
        }

        // PUT: api/TransferirSaldoInsumo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TransferirSaldoInsumo/5
        public void Delete(int id)
        {
        }
    }
}
