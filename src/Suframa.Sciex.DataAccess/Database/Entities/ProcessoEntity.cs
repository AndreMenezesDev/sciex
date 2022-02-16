using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("SCIEX_PROCESSO")]
    public partial class ProcessoEntity : BaseEntity
    {
		public virtual ICollection<PRCStatusEntity> ListaStatus { get; set; }

		public virtual ICollection<PRCSolicProrrogacaoEntity> ListaSolicitacoesProrrogacao { get; set; }
		public virtual ICollection<PRCProdutoEntity> ListaProduto { get; set; }
		public virtual ICollection<ParecerTecnicoEntity> ListaParecerTecnico { get; set; }
		public virtual ICollection<PRCSolicitacaoAlteracaoEntity> ListaSolicitacaoAlteracao { get; set; }
		
		public ProcessoEntity()
		{
			ListaStatus = new HashSet<PRCStatusEntity>();
			ListaProduto = new HashSet<PRCProdutoEntity>();
			ListaParecerTecnico = new HashSet<ParecerTecnicoEntity>();
			ListaSolicitacaoAlteracao = new HashSet<PRCSolicitacaoAlteracaoEntity>();
			ListaSolicitacoesProrrogacao = new HashSet<PRCSolicProrrogacaoEntity>();
		}

		[Key]
		[Column("prc_id")]
		public int IdProcesso { get; set; }		

		[Column("prc_nu")]
		public int? NumeroProcesso { get; set; }
		
		[Column("prc_nu_ano")]
		public int? AnoProcesso { get; set; }

		[Column("prc_nu_inscricao_suframa")]
		public int? InscricaoSuframa { get; set; }

		[StringLength(100)]
		[Column("prc_ds_razao_social")]
		public string RazaoSocial { get; set; }

		[StringLength(1)]
		[Column("prc_tp_modalidade")]
		public string TipoModalidade { get; set; }

		[StringLength(2)]
		[Column("prc_tp_status")]
		public string TipoStatus { get; set; }
		
		[Column("prc_dt_validade")]
		public DateTime? DataValidade { get; set; }
		
		[Column("prc_vl_premio")]
		public decimal? ValorPremio { get; set; }
		
		[Column("prc_vl_perc_ind_imp")]
		public decimal? ValorPercentualIndImportado { get; set; }

		[Column("prc_vl_perc_ind_nac")]
		public decimal? ValorPercentualIndNacional { get; set; }

		[StringLength(14)]
		[Column("prc_nu_cnpj")]
		public string Cnpj { get; set; }

	}
}