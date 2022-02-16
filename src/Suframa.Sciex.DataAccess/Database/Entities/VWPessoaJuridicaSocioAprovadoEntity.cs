using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("VW_PESSOA_JURIDICA_SOCIO_APROVADO")]
	public partial class VWPessoaJuridicaSocioAprovadoEntity : BaseEntity
	{
		[Column("SOC_CO_CNPJ_CPF")]
		[StringLength(14)]
		public string CnpjCpf { get; set; }

		[Column("NJU_ID")]
		[ForeignKey(nameof(NaturezaJuridica))]
		public int? IdNaturezaJuridica { get; set; }

		[Column("PAI_ID")]
		[ForeignKey(nameof(Pais))]
		public int? IdPais { get; set; }

		[Column("PJU_ID")]
		[ForeignKey(nameof(PessoaJuridica))]
		public int IdPessoaJuridica { get; set; }

		[Column("QUA_ID")]
		[ForeignKey(nameof(Qualificacao))]
		public int? IdQualificacao { get; set; }

		[Key]
		[Column("SOC_ID")]
		public int IdSocio { get; set; }

		public virtual NaturezaJuridicaEntity NaturezaJuridica { get; set; }

		[Required]
		[StringLength(100)]
		[Column("SOC_NO")]
		public string Nome { get; set; }

		public virtual PaisEntity Pais { get; set; }

		public virtual PessoaJuridicaEntity PessoaJuridica { get; set; }

		public virtual QualificacaoEntity Qualificacao { get; set; }

		[Column("SOC_TP_PESSOA", TypeName = "numeric")]
		public decimal TipoPessoa { get; set; }

		[Column("SOC_TP_SOCIO", TypeName = "numeric")]
		public int? TipoSocio { get; set; }

		[Column("SOC_VL_PARTICIPACAO")]
		public double? ValorParticipacao { get; set; }
	}
}