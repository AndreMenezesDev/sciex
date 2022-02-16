using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("VW_PESSOA_JURIDICA_APROVADO")]
	public partial class VWPessoaJuridicaAprovadoEntity : BaseEntity, IData
	{
		public virtual CepEntity Cep { get; set; }

		[Column("PJU_CO_CNPJ")]
		[StringLength(14)]
		public string Cnpj { get; set; }

		[StringLength(100)]
		[Column("PJU_DS_COMPLEMENTO")]
		public string Complemento { get; set; }

		public virtual ICollection<CredenciamentoEntity> Credenciamento { get; set; }

		[Column("PJU_DT_ALTERACAO")]
		public DateTime DataAlteracao { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		[Column("PJU_DT_INCLUSAO")]
		public DateTime DataInclusao { get; set; }

		[Column("PJU_DT_REGISTRO")]
		public DateTime? DataRegistro { get; set; }

		public virtual ICollection<DiligenciaEntity> Diligencia { get; set; }

		[StringLength(100)]
		[Column("PJU_EM")]
		public string Email { get; set; }

		[Column("CEP_ID")]
		[ForeignKey(nameof(Cep))]
		public int? IdCep { get; set; }

		[Column("NJU_ID")]
		[ForeignKey(nameof(NaturezaJuridica))]
		public int IdNaturezaJuridica { get; set; }

		[Key]
		[Column("PJU_ID")]
		public int IdPessoaJuridica { get; set; }

		[Column("PEM_ID")]
		[ForeignKey(nameof(PorteEmpresa))]
		public int? IdPorteEmpresa { get; set; }

		public virtual ICollection<InscricaoCadastralEntity> InscricaoCadastral { get; set; }

		public virtual NaturezaJuridicaEntity NaturezaJuridica { get; set; }

		[StringLength(100)]
		[Column("PJU_DS_NOME_FANTASIA")]
		public string NomeFantasia { get; set; }

		[StringLength(10)]
		[Column("PJU_NU_ENDERECO")]
		public string NumeroEndereco { get; set; }

		[StringLength(20)]
		[Column("PJU_NU_INS_MUNICIPAL")]
		public string NumeroInscricaoMunicipal { get; set; }

		[StringLength(20)]
		[Column("PJU_NU_REGISTRO_CONTITUICAO")]
		public string NumeroRegistroConstituicao { get; set; }

		public virtual ICollection<PessoaJuridicaAdministradorEntity> PessoaJuridicaAdministrador { get; set; }

		[Column("PJU_ST_APROVADO")]
		public bool PessoaJuridicaAprovado { get; set; }

		public virtual ICollection<PessoaJuridicaAtividadeEntity> PessoaJuridicaAtividade { get; set; }

		public virtual ICollection<PessoaJuridicaInscricaoEstadualEntity> PessoaJuridicaInscricaoEstadual { get; set; }

		public virtual ICollection<PessoaJuridicaSocioEntity> PessoaJuridicaSocio { get; set; }

		[StringLength(100)]
		[Column("PJU_DS_PONTO_REFERENCIA")]
		public string PontoReferencia { get; set; }

		public virtual PorteEmpresaEntity PorteEmpresa { get; set; }

		public virtual ICollection<QuadroPessoalEntity> QuadroPessoal { get; set; }

		[Column("PJU_NU_RAMAL")]
		public int? Ramal { get; set; }

		[StringLength(100)]
		[Column("PJU_DS_RAZAO_SOCIAL")]
		public string RazaoSocial { get; set; }

		public virtual ICollection<RequerimentoEntity> Requerimento { get; set; }

		[Column("PJU_ST_OP_SIMPLES")]
		public bool? StatusOptanteSimples { get; set; }

		[Column("PJU_NU_TELEFONE")]
		public decimal? Telefone { get; set; }

		[Column("PJU_TP_ENTIDADE_REGISTRO")]
		public int TipoEntidadeRegistro { get; set; }

		[Column("PJU_TP_ESTABELECIMENTO", TypeName = "numeric")]
		public int TipoEstabelecimento { get; set; }

		[Column("PJU_VL_CAPITAL_SOCIAL")]
		public double? ValorCapitalSocial { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public VWPessoaJuridicaAprovadoEntity()
		{
			Credenciamento = new HashSet<CredenciamentoEntity>();
			PessoaJuridicaAdministrador = new HashSet<PessoaJuridicaAdministradorEntity>();
			PessoaJuridicaAtividade = new HashSet<PessoaJuridicaAtividadeEntity>();
			PessoaJuridicaInscricaoEstadual = new HashSet<PessoaJuridicaInscricaoEstadualEntity>();
			PessoaJuridicaSocio = new HashSet<PessoaJuridicaSocioEntity>();
			QuadroPessoal = new HashSet<QuadroPessoalEntity>();
			Requerimento = new HashSet<RequerimentoEntity>();
			InscricaoCadastral = new HashSet<InscricaoCadastralEntity>();
			Diligencia = new HashSet<DiligenciaEntity>();
		}
	}
}