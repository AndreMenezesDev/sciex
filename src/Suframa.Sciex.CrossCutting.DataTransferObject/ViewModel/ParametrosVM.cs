using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ParametrosVM : PagedOptions
	{
		
		public int? IdParametro { get; set; }
		public int? IdMoeda { get; set; }
		public int? IdIncoterms { get; set; }
		public int? IdUnidadeReceitaFederalEntrada { get; set; }
		public int? IdUnidadeReceitaFederalDespacho { get; set; }
		public int? IdFornecedor { get; set; }
		public int? IdFabricante { get; set; }
		public int? IdAladi { get; set; }
		public int? IdNaladi { get; set; }
		public int? IdRegimeTributario { get; set; }
		public int? IdFundamentoLegal { get; set; }
		public int? IdModalidadePagamento { get; set; }
		public int? IdMotivo { get; set; }
		public int? IdInstituicaoFinanceira { get; set; }
		public string Descricao { get; set; }
		public int? TipoCorbeturaCambial { get; set; }
		public int? QuantidadeDiaLimite { get; set; }
		public byte? TipoAcordoTarifario { get; set; }
		public short? TipoFornecedor { get; set; }
		public string CodigoPaiMercadoria { get; set; }
		public string DescricaoPaiMercadoria { get; set; }
		public string CodigoPaisOrigemFabricante { get; set; }
		public string DescricaoPaisOrigemFabricante { get; set; }
		public string CPNJImportador { get; set; }
		public byte[] RowVersion { get; set; }
		public int Codigo { get; set; }

		//complemento de classe
		public int? Id { get; set; }
	}
}