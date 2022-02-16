using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ProcessoDetalheInsumoVM : PagedOptions
	{
		public int IdDetalhe { get; set; }
		public int? NumeroSeqDetalhe { get; set; }
		public decimal? QuantidadeDetalhe { get; set; }
		public decimal ValorUnitarioDetalhe { get; set; }
		public decimal ValorFreteDetalhe { get; set; }
		public decimal ValorDolarDetalhe { get; set; }
		public decimal ValorUnitarioCfrDetalhe { get; set; }
		public decimal ValorDolarCfrDetalhe { get; set; }
		public decimal ValorDolarFobDetalhe { get; set; }
		public int? IdMoe { get; set; }
		public int IdInstituicao { get; set; }
		public decimal ValorNacionalDetalhe { get; set; }


		//variáveis de apoio
		public DateTime? DataEnvioInicial { get; set; }
		public DateTime? DataEnvioFinal { get; set; }
		public int TipoDeConsulta { get; set; }
		public long NumeroAli { get; set; }
		public string DescricaoStatusEnvioSiscomex { get; set; }

	}
}
