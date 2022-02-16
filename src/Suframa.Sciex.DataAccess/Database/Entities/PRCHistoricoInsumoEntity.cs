using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("SCIEX_PRC_HISTORICO_INSUMO")]
    public partial class PRCHistoricoInsumoEntity : BaseEntity
    {
		public PRCHistoricoInsumoEntity()
		{
			ListaDetalheHistoricoInsumo = new HashSet<PRCDetalheHistoricoInsumoEntity>();
		}

		public virtual PRCSolicitacaoAlteracaoEntity SolicitacaoAlteracao { get; set; }
		public virtual PRCProdutoEntity Produto { get; set; }
		public virtual ICollection<PRCDetalheHistoricoInsumoEntity> ListaDetalheHistoricoInsumo { get; set; }


		[Key]
		[Column("his_id")]
		public int IdPRCHistoricoInsumo { get; set; }

		[ForeignKey(nameof(Produto))]
		[Column("pro_id")]
		public int? IdProduto { get; set; }

		[ForeignKey(nameof(SolicitacaoAlteracao))]
		[Column("soa_id")]
		public int? IdSolicitacaoAlteracao { get; set; }

		[Column("his_dt")]
		public DateTime? DataHistorico { get; set; }

		[StringLength(100)]
		[Column("his_no_responsavel")]
		public string NomeResponsavel { get; set; }

		[StringLength(300)]
		[Column("his_ds_insumo")]
		public string DescricaoInsumo { get; set; }

		[StringLength(150)]
		[Column("his_ds_empresa")]
		public string DescricaoEmpresa { get; set; }

		[StringLength(10)]
		[Column("his_ds_processo")]
		public string DescricaoProcesso { get; set; }

		[StringLength(10)]
		[Column("his_ds_solicitacao")]
		public string DescricaoSolicitacao { get; set; }

		[StringLength(300)]
		[Column("his_ds_produto")]
		public string DescricaoProduto { get; set; }

		[StringLength(20)]
		[Column("his_co_insumo")]
		public string CodigoInsumo { get; set; }

	}
}