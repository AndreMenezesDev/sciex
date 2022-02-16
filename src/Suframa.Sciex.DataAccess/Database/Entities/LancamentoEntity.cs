using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_LANCAMENTO")]
	public partial class LancamentoEntity : BaseEntity
	{

		public virtual PliEntity Pli { get; set; }
		public virtual PliMercadoriaEntity PliMercadoria { get; set; }
		public virtual CodigoLancamentoEntity CodigoLancamento { get; set; }

		[Key]
		[Column("LAN_ID")]
		public int IdLancamento { get; set; }
		
		[Required]
		[ForeignKey(nameof(Pli))]
		[Column("PLI_ID")]
		public long IdPli { get; set; }

		[Required]
		[ForeignKey(nameof(PliMercadoria))]
		[Column("PME_ID")]
		public long IdPliMercadoria { get; set; }

		[ForeignKey(nameof(CodigoLancamento))]
		[Column("CLA_ID")]
		public short? IdCodigoLancamento { get; set; }

		[Required]
		[StringLength(1000)]
		[Column("LAN_DS_OBSERVACAO")]
		public string Observacao { get; set; }

		[Required]		
		[Column("LAN_DH_CADASTRO")]
		public DateTime DataCadastro { get; set; }
		
		[Required]
		[StringLength(14)]
		[Column("LAN_NU_CPFCNPJ_RESPONSAVEL")]
		public string NumeroResponsavel { get; set; }

		[Required]
		[Column("LAN_CO_UNIDADE_CADASTRADORA")]
		public int CodigoUnidadeCadastradora { get; set; }

	}
}