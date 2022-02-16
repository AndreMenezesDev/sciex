using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_SOLIC_PE_LOTE")]
	public class SolicitacaoPELoteEntity : BaseEntity
	{

		public SolicitacaoPELoteEntity()
		{
			produtos = new HashSet<SolicitacaoPEProdutoEntity>();
		}

		[Key]
		[Column("slo_id")]
		public int Id { get; set; }
		[Column("slo_nu_lote")]
		public int? NumeroLote { get; set; }
		[Column("slo_tp_modalidade")]
		public string TipoModalidade { get; set; }
		[Column("slo_tp_exportacao")]
		public string TipoExportacao { get; set; }
		[Column("slo_st")]
		public int Situacao { get; set; }
		[Column("slo_nu_inscricao_cadastral")]
		public string InscricaoCadastral { get; set; }
		[Column("slo_nu_cnpj")]
		public string NumeroCNPJ { get; set; }
		[Column("slo_ds_razao_social")]
		public string RazaoSocial { get; set; }
		[Column("slo_nu_ano_lote")]
		public int? Ano { get; set; }
		[Column("slo_nu_processo")]
		public string NumeroProcesso { get; set; }
		[Column("slo_nu_ano_processo")]
		public int? AnoProcesso { get; set; }
		[Column("slo_nu_inscsuf_exportador")]
		public string InscricaoCadastralExportador { get; set; }
		[Column("esp_id")]
		[ForeignKey(nameof(EstruturaPropria))]
		public long EspId { get; set; }

		public virtual EstruturaPropriaPliEntity EstruturaPropria { get; set; }
		public virtual ICollection<SolicitacaoPEProdutoEntity> produtos { get; set; }

	}
}
