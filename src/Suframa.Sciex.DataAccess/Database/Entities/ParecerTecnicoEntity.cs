using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("SCIEX_PARECER_TECNICO")]
    public partial class ParecerTecnicoEntity : BaseEntity
    {
		public virtual ICollection<ParecerTecnicoProdutoEntity> ParecerTecnicoProdutos { get; set; }
		public virtual ParecerComplementarEntity ParecerComplementar { get; set; }
		public virtual ProcessoEntity Processo { get; set; }
		public virtual PRCSolicitacaoAlteracaoEntity SolicitacaoAlteracao { get; set; }

		public ParecerTecnicoEntity()
		{
			ParecerTecnicoProdutos = new HashSet<ParecerTecnicoProdutoEntity>();
		}

		[Key]
		[Column("pat_id")]
		public long IdParecerTecnico { get; set; }

		[ForeignKey(nameof(ParecerComplementar))]
		[Column("pac_id")]
		public int? IdParecerComplementar { get; set; }

		[ForeignKey(nameof(Processo))]
		[Column("prc_id")]
		public int? IdProcesso { get; set; }
		
		[ForeignKey(nameof(SolicitacaoAlteracao))]
		[Column("soa_id")]
		public int? IdSolicAlteracao { get; set; }
		
		[Required]
		[Column("pat_nu_ano_controle")]
		public int AnoControle { get; set; }

		[Required]
		[Column("pat_nu_controle")]
		public long NumeroControle { get; set; }

		[Column("pat_nu_inscricao_cadastral")]
		public int? InscricaoSuframa { get; set; }

		[StringLength(14)]
		[Column("pat_nu_cnpj")]
		public string Cnpj { get; set; }

		[StringLength(100)]
		[Column("pat_ds_razao_social")]
		public string RazaoSocial { get; set; }

		[Column("pat_nu_ano_processo")]
		public int? AnoProcesso { get; set; }

		[Column("pat_nu_processo")]
		public int? NumeroProcesso { get; set; }

		[Column("pat_nu_ano_plano")]
		public int? AnoPlano { get; set; }

		[Column("pat_nu_plano")]
		public int? NumeroPlano { get; set; }

		[StringLength(1)]
		[Column("pat_tp_modalidade")]
		public string TipoModalidade { get; set; }

		[StringLength(2)]
		[Column("pat_tp_status")]
		public string TipoStatus { get; set; }

		[Column("pat_dt_status")]
		public DateTime? DataStatus { get; set; }

		[StringLength(50)]
		[Column("pat_ds_objeto")]
		public string DescricaoObjeto { get; set; }

		[Column("pat_dt_validade")]
		public DateTime? DataValidade { get; set; }

		[Column("pat_qt_total_produto")]
		public decimal? QuantidadeTotalProduto { get; set; }

		[Column("pat_vl_taxa_cambial")]
		public decimal? ValorTaxaCambial { get; set; }

		[Column("pat_vl_indice_nacionalizacao")]
		public decimal? ValorIndiceNacionalizacao { get; set; }

		[Column("pat_vl_indice_importacao")]
		public decimal? ValorIndiceImportacao { get; set; }

		[Column("pat_vl_insumo_nacional")]
		public decimal? ValorInsumoNacional { get; set; }

		[Column("pat_vl_insumo_importado_fob")]
		public decimal? ValorInsumoImportadoFob { get; set; }

		[Column("pat_vl_insumo_importado_cfr")]
		public decimal? ValorInsumoImportadoCfr { get; set; }

		[Column("pat_vl_insumo_importado_real")]
		public decimal? ValorInsumoImportadoReal { get; set; }

		[Column("pat_vl_total_insumos_real")]
		public decimal? ValorTotalInsumosReal { get; set; }

		[Column("pat_vl_exportacao_aprov")]
		public decimal? ValorExportacaoAprovado { get; set; }

		[Column("pat_qt_exportacao_aprov")]
		public decimal? QuantidadeExportacaoAprovado { get; set; }

		[StringLength(11)]
		[Column("pat_nu_cpf_responsavel")]
		public string CpfResponsavel { get; set; }

		[StringLength(100)]
		[Column("pat_no_responsavel")]
		public string NomeResponsavel { get; set; }

		[Column("pat_dt_geracao")]
		public DateTime? DataGeracao { get; set; }

		[Column("pat_vl_importado_autorizado")]
		public decimal? ValorImportadoAutorizado { get; set; }
		
		[Column("pat_vl_importado_acrescimo")]
		public decimal? ValorImportadoAcrescimo { get; set; }
		
		[Column("pat_vl_importado_internado")]
		public decimal? ValorImportadoInternado { get; set; }
		
		[Column("pat_vl_importado_aprovado")]
		public decimal? ValorImportadoAprovado { get; set; }
		
		[Column("pat_vl_importado_frete")]
		public decimal? ValorImportadoFrete { get; set; }
		
		[Column("pat_vl_importado")]
		public decimal? ValorImportado { get; set; }
		
		[Column("pat_vl_nacional_adquirido")]
		public decimal? ValorNacionalAdquirido { get; set; }
		
		[Column("pat_vl_exportacao_realizada")]
		public decimal? ValorExportacaoRealizada { get; set; }
		
		[Column("pat_qt_exportada_unidade")]
		public decimal? QuantidadeExportadaUnidade { get; set; }
		
		[Column("pat_vl_autorizado_internado")]
		public decimal? ValorAutorizadoInternado { get; set; }
		
		[Column("pat_vl_aprovado_autorizado")]
		public decimal? ValorAprovadoAutorizado { get; set; }
		
		[Column("pat_vl_cancelamento_geral")]
		public decimal? ValorCancelamentoGeral { get; set; }

	}
}