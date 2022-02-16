using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_ALI")]
	public partial class AliEntity : BaseEntity
	{

		public virtual PliMercadoriaEntity PliMercadoria { get; set; }
		public virtual AliArquivoEnvioEntity AliArquivoEnvio { get; set; }
					
		[Key]
		[Column("PME_ID")]
		[ForeignKey(nameof(PliMercadoria))]
		public long IdPliMercadoria { get; set; }
		
		[Column("AAE_ID")]
		[ForeignKey(nameof(AliArquivoEnvio))]
		public long? IdAliArquivoEnvio { get; set; }

		[Required]
		[Column("ALI_NU")]
		public long NumeroAli { get; set; }

		[Required]
		[Column("ALI_ST")]
		public byte Status { get; set; }

		[Required]
		[Column("ALI_TP")]
		public byte TipoAli { get; set; }

		[Required]
		[Column("ALI_DH_CADASTRO")]
		public DateTime DataCadastro { get; set; }

		[Column("ALI_DH_CANCELAMENTO")]
		public DateTime? DataCancelamento { get; set; }

		[Column("ALI_DH_PROCESSAMENTO_SUFRAMA")]
		public DateTime? DataProcessamentoSuframa { get; set; }

		[Column("ALI_DH_RESPOSTA_SISCOMEX")]
		public DateTime? DataRespostaSISCOMEX { get; set; }

		[Column("AAE_DS_NOME_ARQUIVO")]
		[StringLength(255)]
		public string NomeArquivo { get; set; }

		[Column("ali_nu_ativar_origem")]
		public byte? NumeroAtivaOrigem { get; set; }
		
	}
}