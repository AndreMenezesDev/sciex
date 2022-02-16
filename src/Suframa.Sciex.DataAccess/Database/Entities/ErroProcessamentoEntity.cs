using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_ERRO_PROCESSAMENTO")]
	public partial class ErroProcessamentoEntity : BaseEntity
	{

		public virtual PliEntity Pli { get; set; }		
		public virtual ErroMensagemEntity ErroMensagem { get; set; }
		public virtual SolicitacaoPliEntity SolicitacaoPli { get; set; }
		public virtual DiEntradaEntity DiEntrada { get; set; }
		public virtual EstruturaPropriaLEEntity EstruturaPropriaLE { get; set; }

		[Key]
		[Column("EPR_ID")]
		public int IdErroProcessamento { get; set; }

		[ForeignKey(nameof(Pli))]
		[Column("PLI_ID")]
		public long? IdPli { get; set; }

		[ForeignKey(nameof(SolicitacaoPli))]
		[Column("SPL_ID")]
		public long? IdSolicitacaoPli { get; set; }

		[ForeignKey(nameof(ErroMensagem))]
		[Column("EME_ID")]
		public short? IdErroMensagem { get; set; }

		[ForeignKey(nameof(EstruturaPropriaLE))]
		[Column("ESP_ID")]
		public long? IdEstruturaPropriaLE { get; set; }

		[Column("SLO_ID")]
		public int? IdLote { get; set; }

		[Column("EPR_CO_NIVEL_ERRO")]
		public byte? CodigoNivelErro { get; set; }

		[Column("EPR_ID_MERC_DETALHE")]
		public long? IdPliMercadoriaOuPliDetalheMercadoria { get; set; }

		[Required]
		[StringLength(500)]
		[Column("EPR_DS_MENSAGEM_ERRO")]
		public string Descricao { get; set; }

		[Column("EPR_DH_PROCESSAMENTO")]
		public DateTime? DataProcessamento { get; set; }

		[Column("SPL_NU_CNPJ")]
		[StringLength(14)]
		public string CNPJImportador { get; set; }

		[Column("SPL_NU_PLI_IMPORTADOR")]
		[StringLength(10)]
		public string NumeroPLIImportador { get; set; }

		[ForeignKey(nameof(DiEntrada))]
		[Column("DIE_ID")]
		public long? IdDiEntrada { get; set; }
	}
}