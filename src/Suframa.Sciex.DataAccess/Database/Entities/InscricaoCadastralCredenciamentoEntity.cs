using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("VM_CADSUF_INSCRICAO_CADASTRAL_CREDENCIAMENTO")]
	public class InscricaoCadastralCredenciamentoEntity : BaseEntity
	{
		[Column("CpfCnpj")]
		public string CpfCnpj { get; set; }

		public virtual CredenciamentoEntity Credenciamento { get; set; }

		[Column("DataAbertura")]
		public DateTime? DataAbertura { get; set; }

		[Column("DataValidade")]
		public DateTime? DataValidade { get; set; }

		[Column("DescricaoStatus")]
		public string DescricaoStatus { get; set; }

		[Column("DescricaoTipoUsuario")]
		public string DescricaoTipoUsuario { get; set; }

		[Column("DescricaoUnidadeCadastradora")]
		public string DescricaoUnidadeCadastradora { get; set; }

		[Column("CRE_ID")]
		[ForeignKey(nameof(Credenciamento))]
		public int? IdCredenciamento { get; set; }

		[Column("INS_ID")]
		[ForeignKey(nameof(InscricaoCadastral))]
		public int? IdInscricaoCadastral { get; set; }

		[Column("INS_CRE_ID")]
		[Key]
		public long IdInscricaoCadastralCredenciamento { get; set; }

		[Column("PFI_ID")]
		public int? IdPessoaFisica { get; set; }

		[Column("PJU_ID")]
		public int? IdPessoaJuridica { get; set; }

		[Column("TUS_ID")]
		[ForeignKey(nameof(TipoUsuario))]
		public int? IdTipoUsuario { get; set; }

		[Column("InscricaoCadastral")]
		public int? Inscricao { get; set; }

		public virtual InscricaoCadastralEntity InscricaoCadastral { get; set; }

		[Column("NomeRazaoSocial")]
		public string NomeRazaoSocial { get; set; }

		public virtual TipoUsuarioEntity TipoUsuario { get; set; }
	}
}