using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("VW_SCIEX_IMPORTADOR")]
    public partial class ViewImportadorEntity : BaseEntity
    {

		[Key]
		[Column("PJU_ID")]
		public int IdPessoaJuridica { get; set; }

		[StringLength(14)]
		[Column("imp_nu_cnpj")]
		public string Cnpj { get; set; }

		[StringLength(60)]
		[Column("imp_ds_razao_social")]
		public string RazaoSocial { get; set; }

		[StringLength(60)]
		[Column("PJU_DS_NOME_FANTASIA")]
		public string NomeFantasia { get; set; }

		[StringLength(15)]
		[Column("PJU_TP_ENTIDADE_REGISTRO")]
		public string TipoEntidadeRegistro { get; set; }

		[StringLength(6)]
		[Column("PJU_TP_ESTABELECIMENTO")]
		public string TipoEstabelecimento { get; set; }

		[StringLength(20)]
		[Column("PJU_NU_REGISTRO_CONTITUICAO")]
		public string NumeroRegistroConstituicao { get; set; }
		
		[Column("PJU_ST_OP_SIMPLES")]
		public bool? StatusOptanteSimples { get; set; }

		[StringLength(3)]
		[Column("PJU_ST_OP_SIMPLES_DESC")]
		public string StatusOptanteSimplesDescricao { get; set; }
		
		[Column("PJU_VL_CAPITAL_SOCIAL")]
		public double? ValorCapitalSocial { get; set; }

		[Column("CEP_CO")]
		public decimal CEP { get; set; }

		[StringLength(100)]
		[Column("CEP_TP_LOGRADOURO")]
		public string Logradouro { get; set; }

		[StringLength(40)]
		[Column("CEP_DS_ENDERECO")]
		public string Endereco { get; set; }

		[StringLength(25)]
		[Column("CEP_DS_BAIRRO")]
		public string Bairro { get; set; }

		[StringLength(21)]
		[Column("PJU_DS_COMPLEMENTO")]
		public string Complemento { get; set; }

		[StringLength(6)]
		[Column("PJU_NU_ENDERECO")]
		public string Numero { get; set; }

		[StringLength(21)]
		[Column("PJU_DS_PONTO_REFERENCIA")]
		public string PontoReferencia { get; set; }
		
		[Column("MUN_CO")]
		public decimal CodigoMunicipio { get; set; }

		[StringLength(25)]
		[Column("MUN_DS")]
		public string Municipio { get; set; }
		
		[Column("MUN_CO_UF")]
		public int? CodigoUF { get; set; }

		[StringLength(2)]
		[Column("MUN_SG_UF")]
		public string UF { get; set; }

		[StringLength(15)]
		[Column("PJU_NU_TELEFONE")]
		public string Telefone { get; set; }

		[StringLength(15)]
		[Column("PJU_NU_RAMAL")]
		public string Ramal { get; set; }

		[StringLength(50)]
		[Column("PJU_EM")]
		public string Email { get; set; }
		
		[Column("NJU_ID")]
		public int IdNaturezaJuridica { get; set; }

		[StringLength(500)]
		[Column("NJU_DS")]
		public string DescricaoNaturezaJuridica { get; set; }
	
		[Column("NGR_ID")]
		public int IdNaturezaGrupo { get; set; }

		[Column("NGR_CO")]
		public int CodigoNaturezaGrupo { get; set; }

		[StringLength(200)]
		[Column("NGR_DS")]
		public string DescricaoNaturezaGrupo { get; set; }

		[Column("CEP_ID")]
		public int? IdCEP { get; set; }

		[Column("PEM_ID")]
		public int? IdPorteEmpresa { get; set; }

		[StringLength(100)]
		[Column("PEM_DS")]
		public string DescricaoPorteEmpresa { get; set; }

		[StringLength(20)]
		[Column("PJU_NU_INS_MUNICIPAL")]
		public string NumeroInscricaoMunicipal { get; set; }
		
		[Column("INS_CO")]
		public int InscricaoCadastral { get; set; }

		[Column("UND_ID")]
		public int IdUnidadeCadastradora { get; set; }

		[Column("UND_CO")]
		public int? CodigoUnidadeCadastradora { get; set; }

		[StringLength(50)]
		[Column("UND_DS")]
		public string DescricaoUnidadeCadastradora { get; set; }
		
		[Column("STI_ID")]
		public int IdSituacaoInscricao { get; set; }

		[StringLength(40)]
		[Column("STI_DS")]
		public string DescricaoSituacaoInscricao { get; set; }
	
		[Column("INS_DT")]
		public DateTime DataInscricaoCadastral { get; set; }

		[Column("INS_ST_PROJETO_APROVADO")]
		public int? StatusInscricaoCadastalProjetoAprovado { get; set; }

		[StringLength(3)]
		[Column("PROJETO_APROVADO")]
		public string TemProjetoAprovado { get; set; }

		[Column("NGR_COBRAR_TCIF")]
		public int CobrarTCIF { get; set; }

		[StringLength(3)]
		[Column("PAI_CO")]
		public string CodigoPais { get; set; }

		[StringLength(6)]
		[Column("PAI_DS")]
		public string DescricaoPais { get; set; }

	}
}