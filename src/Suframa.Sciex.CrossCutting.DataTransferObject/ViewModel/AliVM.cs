using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class AliVM : PagedOptions
	{
		public long IdPliMercadoria { get; set; }
		public long? IdAliArquivoEnvio { get; set; }
		public long NumeroAli { get; set; }
		public byte Status { get; set; }
		public byte TipoAli { get; set; }
		public DateTime DataCadastro { get; set; }
		public DateTime? DataCancelamento { get; set; }
		public DateTime? DataProcessamentoSuframa { get; set; }
		public DateTime? DataRespostaSISCOMEX { get; set; }
		public string NomeArquivo { get; set; }


		//Complemento de Classe
		public long IdPli { get; set; }
		public string DescricaoStatus { get; set; }
		public string NumeroPliConcatenado { get; set; }
		public string NumeroLi { get; set; }
		public string NomenclaturaComumMercosul { get; set; }
		public string CodigoProduto { get; set; }
		public string TipoProduto { get; set; }
		public string CodigoModeloProduto { get; set; }
		
		public string QuantidadeErroAli { get; set; }
	}
}
