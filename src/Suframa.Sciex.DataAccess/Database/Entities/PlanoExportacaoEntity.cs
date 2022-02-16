using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PLANO_EXPORTACAO")]
	public partial class PlanoExportacaoEntity : BaseEntity
	{
		public virtual ICollection<PEArquivoEntity> ListaAnexos { get; set; }
		public virtual ICollection<PEProdutoEntity> ListaPEProdutos { get; set; }
		public virtual ICollection<PEHistoricoEntity> ListaPEHistoricos { get; set; }
		public PlanoExportacaoEntity()
		{
			ListaAnexos = new HashSet<PEArquivoEntity>();
			ListaPEProdutos = new HashSet<PEProdutoEntity>();
			ListaPEHistoricos = new HashSet<PEHistoricoEntity>();
		}

		[Key]
		[Column("PEX_ID")]
		public int IdPlanoExportacao { get; set; }

		[Column("PEX_NU_PLANO")]
		public long NumeroPlano { get; set; }

		[Column("PEX_NU_ANO_PLANO")]
		public int AnoPlano { get; set; }

		[Column("PEX_NU_INSCRICAO_CADASTRAL")]
		public int? NumeroInscricaoCadastral { get; set; }

		[Column("PEX_NU_CNPJ")]
		[StringLength(14)]
		public string Cnpj { get; set; }

		[Column("PEX_DS_RAZAO_SOCIAL")]
		[StringLength(100)]
		public string RazaoSocial { get; set; }

		[Column("PEX_TP_MODALIDADE")]
		[StringLength(1)]
		public string TipoModalidade { get; set; }

		[Column("PEX_TP_EXPORTACAO")]
		[StringLength(2)]
		public string TipoExportacao { get; set; }

		[Column("PEX_ST")]
		public int Situacao { get; set; }

		[Column("PEX_DT_ENVIO")]
		public DateTime? DataEnvio { get; set; }

		[Column("PEX_DT_CADASTRO")]
		public DateTime? DataCadastro { get; set; }

		[Column("PEX_DT_STATUS")]
		public DateTime? DataStatus { get; set; }

		[Column("PEX_NU_CPF_RESPONSAVEL")]
		[StringLength(11)]
		public string CpfResponsavel { get; set; }

		[Column("PEX_NO_RESPONSAVEL")]
		[StringLength(100)]
		public string NomeResponsavel { get; set; }

		[Column("PEX_DS_JUSTIFICATIVA_ERRO")]
		[StringLength(1000)]
		public string DescricaoJustificativaErro { get; set; }

		[Column("PEX_NU_PROCESSO")]
		public int? NumeroProcesso { get; set; }

		[Column("PEX_NU_ANO_PROCESSO")]
		public int? NumeroAnoProcesso { get; set; }
	}
}