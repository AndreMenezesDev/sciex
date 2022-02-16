using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_PLI")]
	public partial class PliEntity : BaseEntity
	{
		public virtual PliAplicacaoEntity PliAplicacao { get; set; }
		public virtual EstruturaPropriaPliEntity EstruturaPropriaPli { get; set; }
		public virtual TaxaPliEntity TaxaPli { get; set; }
		public virtual ICollection<PliHistoricoEntity> PLIHistorico { get; set; }
		public virtual ICollection<PliMercadoriaEntity> PLIMercadoria { get; set; }
		public virtual ICollection<PliProdutoEntity> PLIProduto { get; set; }
		public virtual ICollection<LancamentoEntity> Lancamento { get; set; }
		public virtual ICollection<ErroProcessamentoEntity> ErroProcessamento { get; set; }
		public virtual PliAnaliseVisualEntity PliAnaliseVisual { get; set; }

		public PliEntity()
		{
			PLIHistorico = new HashSet<PliHistoricoEntity>();
			PLIMercadoria = new HashSet<PliMercadoriaEntity>();
			PLIProduto = new HashSet<PliProdutoEntity>();		
			Lancamento = new HashSet<LancamentoEntity>();
			ErroProcessamento = new HashSet<ErroProcessamentoEntity>();
		}

		[Key]
		[Column("PLI_ID")]
		public long IdPLI { get; set; }

		[Required]
		[Column("PAP_ID")]
		[ForeignKey(nameof(PliAplicacao))]
		public int IdPLIAplicacao { get; set; }

		[Required]
		[Column("PLI_NU")]
		public long NumeroPli { get; set; }

		[Required]
		[Column("PLI_NU_ANO")]
		public int Ano { get; set; }

		[Required]
		[StringLength(14)]
		[Column("PLI_NU_CNPJ")]
		public string Cnpj { get; set; }

		[Column("INS_CO")]
		public int? InscricaoCadastral { get; set; }

		[Required]
		[Column("SET_CO")]
		public int? CodigoSetor { get; set; }

		[Required]
		[StringLength(200)]
		[Column("SET_DS")]
		public String DescricaoSetor { get; set; }

		[Required]
		[Column("PLI_TP_DOCUMENTO")]
		public byte TipoDocumento { get; set; }

		[Column("PLI_ST_ANALISE_VISUAL")]
		public short? StatusAnaliseVisual { get; set; }

		[Column("PLI_ST_DISTRIBUICAO")]
		public short? StatusDistribuicao { get; set; }

		[Column("PLI_VL_TCIF")]
		public decimal? ValorTCIF { get; set; }

		[Column("PLI_VL_TCIF_ITENS")]
		public decimal? ValorTECIFItens { get; set; }

		[Column("PLI_NU_DEBITO")]
		public int? Debito { get; set; }

		[Column("PLI_NU_DEBITO_ANO")]
		public short? DebitoAno { get; set; }

		[Column("PLI_DH_DEBITO_PGTO")]
		public DateTime? DataDebitoPagamento { get; set; }

		[Column("PLI_DH_DEBITO_GERACAO")]
		public DateTime? DataDebitoGeracao { get; set; }

		[StringLength(10)]
		[Column("PLI_NU_LI_REFERENCIA")]
		public string NumeroLIReferencia { get; set; }

		[StringLength(50)]
		[Column("PLI_NU_DI_REFERENCIA")]
		public string NumeroDIReferencia { get; set; }

		[Column("PLI_NU_PEXPAM")]
		public int? NumeroPEXPAM { get; set; }

		[Column("PLI_NU_ANO_PEXPAM")]
		public short? AnoPEXPAM { get; set; }

		[StringLength(10)]
		[Column("PLI_NU_LOTE_PEXPAM")]
		public string LotePEXPAM { get; set; }

		[StringLength(8000)]
		[Column("PLI_ME_ALI_ARQUIVO")]
		public string MEALIArquivo { get; set; }

		[Column("PLI_TP_ORIGEM")]
		public byte? TipoOrigem { get; set; }

		[Required]
		[StringLength(14)]
		[Column("PLI_NU_RESPONSAVEL_CADASTRO")]
		public string NumeroResponsavelRegistro { get; set; }

		[Required]
		[StringLength(100)]
		[Column("PLI_NO_RESPONSAVEL_CADASTRO")]
		public string NomeResponsavelRegistro { get; set; }

		[Required]
		[Column("PLI_DH_CADASTRO")]
		public DateTime DataCadastro { get; set; }
		
		[Column("PLI_DH_ENVIO")]
		public DateTime? DataEnvioPli { get; set; }

		[Required]
		[StringLength(11)]
		[Column("PLI_NU_CPF_REP_LEGAL_SISCOMEX")]
		public string NumCPFRepLegalSISCO { get; set; }

		[Required]
		[StringLength(10)]
		[Column("PLI_CO_CNAE")]
		public string CodigoCNAE { get; set; }


		[Column("PLI_VL_TOTAL_CONDICAO_VENDA")]
		public decimal? ValorTotalCondicaoVenda { get; set; }

		[Column("PLI_VL_TOTAL_CONDICAO_VENDA_REAL")]
		public decimal? ValorTotalCondicaoVendaReal { get; set; }

		[Column("PLI_VL_TOTAL_CONDICAO_VENDA_DOLAR")]
		public decimal? ValorTotalCondicaoVendaDolar { get; set; }

		[Column("PLI_COL_ROW_VERSION")]
		[Timestamp]
		[ConcurrencyCheck]
		public byte[] RowVersion { get; set; }

		[Required]
		[StringLength(100)]
		[Column("IMP_DS_RAZAO_SOCIAL")]
		public string RazaoSocial { get; set; }

		[Required]		
		[Column("PLI_ST_PLI")]
		public byte StatusPli { get; set; }
		
		[Column("PLI_ST_PROCESSAMENTO")]
		public byte? StatusPliProcessamento { get; set; }

		[StringLength(10)]
		[Column("SPL_NU_PLI_IMPORTADOR")]
		public string NumeroPliImportador { get; set; }

		[Column("SPL_ST_PLI_TECNOLOGIA_ASSISTIVA")]
		public byte? StatusPliTecnologiaAssistiva { get; set; }

		[StringLength(1)]
		[Column("SPL_ST_INDICACAO_PLI_EXIGENCIA")]
		public string StatusIndicacaoPliExigencia { get; set; }

		[Column("ESP_ID")]
		[ForeignKey(nameof(EstruturaPropriaPli))]
		public long? IdEstruturaPropria { get; set; }

		[StringLength(100)]
		[Column("PLI_DS_ARQUIVO")]
		public string NomeAnexo { get; set; }


		[Column("PLI_ME_ARQUIVO")]
		public byte[] Anexo { get; set; }
	}
}