using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("CADSUF_DADOS_SOLICITANTE")]
	public partial class DadosSolicitanteEntity : BaseEntity, IData
	{
		public virtual ICollection<AgendaAtendimentoEntity> AgendaAtendimento { get; set; }

		[Column("DSO_CO_CPF")]
		[StringLength(11)]
		public string Cpf { get; set; }

		[Column("DSO_DT_ALTERACAO")]
		public DateTime DataAlteracao { get; set; }

		[Column("DSO_DT_INCLUSAO")]
		public DateTime DataInclusao { get; set; }

		[Required]
		[StringLength(50)]
		[Column("DSO_EM")]
		public string Email { get; set; }

		[Key]
		[Column("DSO_ID")]
		public int IdSolicitante { get; set; }

		[Required]
		[StringLength(100)]
		[Column("DSO_NO")]
		public string Nome { get; set; }

		[StringLength(100)]
		[Column("DSO_NO_SOCIAL")]
		public string NomeSocial { get; set; }

		[Column("DSO_NU_RAMAL")]
		public int? Ramal { get; set; }

		public virtual ICollection<RequerimentoEntity> Requerimento { get; set; }

		[Column("DSO_NU_TELEFONE")]
		public decimal? Telefone { get; set; }

		public DadosSolicitanteEntity()
		{
			Requerimento = new HashSet<RequerimentoEntity>();
			AgendaAtendimento = new HashSet<AgendaAtendimentoEntity>();
		}
	}
}