using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("CADSUF_SUBCLASSE_ATIVIDADE")]
	public partial class SubclasseAtividadeEntity : BaseEntity, IData
	{
		public virtual ClasseAtividadeEntity ClasseAtividade { get; set; }

		[Column("SBC_CO", TypeName = "numeric")]
		public decimal Codigo { get; set; }

		[StringLength(7)]
		[Column("SBC_CO_CONCLA")]
		public string CodigoCNAE { get; set; }

		[Column("SBC_DT_ALTERACAO")]
		public DateTime DataAlteracao { get; set; }

		[Column("SBC_DT_INCLUSAO")]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime DataInclusao { get; set; }

		[Required]
		[StringLength(200)]
		[Column("SBC_DS")]
		public string Descricao { get; set; }

		[Column("CLA_ID")]
		public int IdClasseAtividade { get; set; }

		[Key]
		[Column("SBC_ID")]
		public int IdSubclasseAtividade { get; set; }

		public virtual ICollection<PessoaJuridicaAtividadeEntity> PessoaJuridicaAtividade { get; set; }

		public virtual ICollection<SetorAtividadeEntity> SetorAtividade { get; set; }

		[Column("SBC_ST")]
		public bool Status { get; set; }

		public SubclasseAtividadeEntity()
		{
			PessoaJuridicaAtividade = new HashSet<PessoaJuridicaAtividadeEntity>();
			SetorAtividade = new HashSet<SetorAtividadeEntity>();
		}
	}
}