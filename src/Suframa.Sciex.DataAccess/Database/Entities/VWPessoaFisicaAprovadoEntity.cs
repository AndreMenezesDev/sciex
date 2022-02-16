using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("VW_PESSOA_FISICA_APROVADO")]
	public partial class VWPessoaFisicaAprovadoEntity : BaseEntity, IData
	{
		public virtual CepEntity Cep { get; set; }

		[StringLength(250)]
		[Column("PFI_DS_COMPLEMENTO")]
		public string Complemento { get; set; }

		public virtual ICollection<ConsultaPublicaEntity> ConsultaPublica { get; set; }

		[Column("PFI_CO_CPF")]
		[StringLength(11)]
		public string Cpf { get; set; }

		public virtual ICollection<CredenciamentoEntity> Credenciamento { get; set; }

		[Column("PFI_DT_ALTERACAO")]
		public DateTime DataAlteracao { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		[Column("PFI_DT_INCLUSAO")]
		public DateTime DataInclusao { get; set; }

		public virtual ICollection<DiligenciaEntity> Diligencia { get; set; }

		[StringLength(100)]
		[Column("PFI_EM")]
		public string Email { get; set; }

		[Column("CEP_ID")]
		[ForeignKey(nameof(Cep))]
		public int IdCep { get; set; }

		[Key]
		[Column("PFI_ID")]
		public int IdPessoaFisica { get; set; }

		public virtual ICollection<InscricaoCadastralEntity> InscricaoCadastral { get; set; }

		[Required]
		[StringLength(100)]
		[Column("PFI_NO")]
		public string Nome { get; set; }

		[StringLength(100)]
		[Column("PFI_NO_SOCIAL")]
		public string NomeSocial { get; set; }

		[Required]
		[StringLength(10)]
		[Column("PFI_NU_ENDERECO")]
		public string NumeroEndereco { get; set; }

		public virtual ICollection<PessoaFisicaAprovadoEntity> PessoaFisicaAprovado { get; set; }

		[StringLength(100)]
		[Column("PFI_DS_PONTO_REFERENCIA")]
		public string PontoReferencia { get; set; }

		[Column("PFI_NU_RAMAL")]
		public int? Ramal { get; set; }

		public virtual ICollection<RequerimentoEntity> Requerimento { get; set; }

		[Column("PFI_ST_APROVADO")]
		public bool StatusAprovado { get; set; }

		[Column("PFI_NU_TELEFONE")]
		public decimal Telefone { get; set; }

		public VWPessoaFisicaAprovadoEntity()
		{
			ConsultaPublica = new HashSet<ConsultaPublicaEntity>();
			Credenciamento = new HashSet<CredenciamentoEntity>();
			Requerimento = new HashSet<RequerimentoEntity>();
			InscricaoCadastral = new HashSet<InscricaoCadastralEntity>();
			Diligencia = new HashSet<DiligenciaEntity>();
			PessoaFisicaAprovado = new HashSet<PessoaFisicaAprovadoEntity>();
		}
	}
}