using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PLI_HISTORICO")]
	public partial class PliHistoricoEntity : BaseEntity
	{
		public virtual PliEntity Pli { get; set; }

		[Key]
		[Column("PHI_ID")]
		public int IdPliHistorico { get; set; }

		[Column("PHI_DH_EVENTO")]
		public DateTime? DataEvento { get; set; }

		[Required]
		[Column("PLI_ID")]
		[ForeignKey(nameof(Pli))]
		public long? IdPLI { get; set; }

		[Column("PHI_NU_CPFCNPJ_RESPONSAVEL")]
		[StringLength(14)]
		public string CPFCNPJ { get; set; }

		[Column("PHI_NO_RESPONSAVEL")]
		[StringLength(80)]
		public string NomeResponsavel { get; set; }

		[Column("PHI_DS_OBSERVACAO")]
		[StringLength(1000)]
		public string Observacao { get; set; }

		[Column("PLI_ST_PLI")]		
		public byte? StatusPli { get; set; }
			
		[Column("PLI_ST_PLI_DESCRICAO")]
		[StringLength(50)]
		public string DescricaoStatusPli { get; set; }

	}
}