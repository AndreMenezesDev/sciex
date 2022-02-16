using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class LiVM : PagedOptions
	{
		public long IdPliMercadoria { get; set; }
		public long? IdPliMercadoriaRefrencia { get; set; }
		public long? IdAliArquivoEnvio { get; set; }
		public long? IdDI { get; set; }
		public long? IdLiArquivoRetorno { get; set; }
		public long? NumeroLi { get; set; }
		public byte Status { get; set; }
		public byte TipoLi { get; set; }
		public byte TipoAli { get; set; }
		public DateTime DataCadastro { get; set; }
		public DateTime? DataCancelamento { get; set; }
		public string MensagemErroLI { get; set; }

		//Complemento de Classe
		public bool AtivarLIOriginal { get; set; }
		public string DescricaoStatus { get; set; }
		public long NumeroPli { get; set; }
		public int AnoPli { get; set; }
		public string NumeroNCM { get; set; }
		public string DescricaoNCMMercadoria { get; set; }
		public List<long> ListaSelecionados { get; set; }
		public bool Checkbox { get; set; }
		public byte StatusAli { get; set; }
		public string MensagemErro { get; set; }
		public string DataCadastroFormatado { get; set; }
		public string NumeroPLIFormatado { get; set; }
		public string CNPJEmpresa { get; set; }
		public string RazaoSocialEmpresa { get; set; }
		public long? NumeroALI { get; set; }
		public string DescricaoStatusAcao { get; set; }
		public string NumeroLiMascarado { get; set; }
		public string NumeroReferencia { get; set; }
		public string DataStatusFormatada { get; set; }
		public long IdPLI { get; set; }


	}
}
