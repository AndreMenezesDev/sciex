using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("CADSUF_PESSOA_JURIDICA_ADMINISTRADOR")]
	public partial class PessoaJuridicaAdministradorEntity : BaseEntity
	{
		public virtual ICollection<ConsultaPublicaEntity> ConsultaPublica { get; set; }

		[Column("ADM_CO_CPF")]
		[StringLength(11)]
		public string Cpf { get; set; }

		[Key]
		[Column("ADM_ID")]
		public int IdAdministrador { get; set; }

		[Column("PJU_ID")]
		[ForeignKey(nameof(PessoaJuridica))]
		public int IdPessoaJuridica { get; set; }

		[Column("QUA_ID")]
		[ForeignKey(nameof(Qualificacao))]
		public int IdQualificacao { get; set; }

		[Required]
		[StringLength(100)]
		[Column("AMD_NO")]
		public string Nome { get; set; }

		[StringLength(100)]
		[Column("ADM_NO_SOCIAL")]
		public string NomeSocial { get; set; }

		public virtual PessoaJuridicaEntity PessoaJuridica { get; set; }

		public virtual QualificacaoEntity Qualificacao { get; set; }

		public PessoaJuridicaAdministradorEntity()
		{
			ConsultaPublica = new HashSet<ConsultaPublicaEntity>();
		}
	}
}