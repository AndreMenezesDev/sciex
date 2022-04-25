using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_SOLIC_PE_PRODUTO_PAIS")]
	public class SolicitacaoPEProdutoPaisEntity : BaseEntity
	{	

		public virtual SolicitacaoPEProdutoEntity SolicitacaoPEProduto { get; set; }
		public virtual ICollection<SolicitacaoPEDueEntity> ListaSolicPEDue { get; set; }

		public SolicitacaoPEProdutoPaisEntity()
		{
			ListaSolicPEDue = new HashSet<SolicitacaoPEDueEntity>();
		}

		[Key]
		[Column("prp_id")]
		public int Id { get; set; }
		[Column("spp_id")]
		[ForeignKey(nameof(SolicitacaoPEProduto))]
		public int ProdutoId { get; set; }
		[Column("prp_co_produto_exp")]
		public int CodigoProdutoPexPam { get; set; }
		[Column("prp_co_pais")]
		public string CodigoPais { get; set; }
		[Column("prp_nu_re")]
		public string RegistroExportacao { get; set; }
		[Column("prp_qt")]
		public decimal? Quantidade { get; set; }
		[Column("prp_vl_dolar")]
		public decimal? ValorDolar { get; set; }
		[Column("prp_nu_lote")]
		public int? NumeroLote { get; set; }
		[Column("prp_nu_ano_lote")]
		public int? AnoLote { get; set; }
		[Column("prp_nu_inscricao_cadastral")]
		public string InscricaoCadastral { get; set; }
		[Column("prp_dt_embarque")]
		public DateTime? DataEmbarque { get; set; }

	}
	/***
	 * 
	 * prp_id

spp_id






prp_dt_embarque
	 * 
	 */
}
