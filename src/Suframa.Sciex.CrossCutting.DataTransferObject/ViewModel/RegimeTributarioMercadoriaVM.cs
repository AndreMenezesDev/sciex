using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class RegimeTributarioMercadoriaVM : PagedOptions
	{
		public int? IdRegimeTributarioMercadoria { get; set; }
		public byte Status { get; set; }
		public int IdRegimeTributario { get; set; }
		public int IdFundamentoLegal { get; set; }
		public int CodigoMunicipio { get; set; }
		public string DescricaoMunicipio { get; set; }
		public string UF { get; set; }
		public DateTime DataInicioVigencia { get; set; }
		public byte[] RowVersion { get; set; }

		public DateTime? DataInicio { get; set; }
		public DateTime? DataFim { get; set; }
		public short CodigoRegimeTributario { get; set; }
		public short CodigoFundamentoLegal { get; set; }
		public string DescricaoRegimeTributario { get; set; }
		public string DescricaoFundamentoLegal { get; set; }
		public string CodigoDescricaoRegimeTributario { get; set; }
		public string CodigoDescricaoFundamentoLegal { get; set; }
		public string CodigoDescricaoMunicipio { get; set; }
		public string codigoDoMunicipio { get; set; }
		public string MensagemErro { get; set; }
		public int isEditStatus { get; set; }
		public string DataVigenciaFormatado { get; set; }


	}
}
