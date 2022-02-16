using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("VW_PESSOA_JURIDICA_ADMINISTRADOR_APROVADO")]
	public partial class VWPessoaJuridicaAdministradorAprovadoEntity : BaseEntity
	{
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

		[Column("AMD_NO")]
		public string Nome { get; set; }

		public virtual PessoaJuridicaEntity PessoaJuridica { get; set; }

		public virtual QualificacaoEntity Qualificacao { get; set; }
	}
}