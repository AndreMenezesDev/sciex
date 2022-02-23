using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ParecerTecnicoVM : PagedOptions
	{
		#region CampoTabela
		public long IdParecerTecnico { get; set; }
		public int? IdProcesso { get; set; }
		public int? IdSolicAlteracao { get; set; }
		public long NumeroControle { get; set; }
		public int AnoControle { get; set; }
		public int? InscricaoSuframa { get; set; }
		public string Cnpj { get; set; }
		public string RazaoSocial { get; set; }
		public int? AnoProcesso { get; set; }
		public int? NumeroProcesso { get; set; }
		public int? AnoPlano { get; set; }
		public int? NumeroPlano { get; set; }
		public string TipoModalidade { get; set; }
		public string TipoStatus { get; set; }
		public DateTime? DataStatus { get; set; }
		public string DescricaoObjeto { get; set; }
		public DateTime? DataValidade { get; set; }
		public decimal? QuantidadeTotalProduto { get; set; }
		public decimal? ValorTaxaCambial { get; set; }
		public decimal? ValorIndiceNacionalizacao { get; set; }
		public decimal? ValorIndiceImportacao { get; set; }
		public decimal? ValorInsumoNacional { get; set; }
		public decimal? ValorInsumoImportadoFob { get; set; }
		public decimal? ValorInsumoImportacoCfr { get; set; }
		public decimal? ValorInsumoImportadoReal { get; set; }
		public decimal? ValorTotalInsumosReal { get; set; }
		public decimal? ValorExportacaoAprovado { get; set; }
		public decimal? QuantidadeExportacaoAprovado { get; set; }
		public decimal? ExportacaoComprovada { get; set; }
		public decimal? InsumosNacionaisAdquiridos { get; set; }
		public decimal? InsumosImportadosAutorizados { get; set; }
		public decimal? TotalInsumosImportadosInternados { get; set; }
		public decimal? ValorImportadoAutorizado { get; set; }
		public decimal? ValorImportadoAcrescimo { get; set; }
		public decimal? ValorImportadoInternado { get; set; }
		public decimal? ValorImportadoAprovado { get; set; }
		public decimal? ValorImportadoFrete { get; set; }
		public decimal? ValorImportado { get; set; }
		public decimal? ValorNacionalAdquirido { get; set; }
		public decimal? ValorExportacaoRealizada { get; set; }
		public decimal? QuantidadeExportadaUnidade { get; set; }
		public decimal? ValorAutorizadoInternado { get; set; }
		public decimal? ValorAprovadoAutorizado { get; set; }
		public decimal? ValorCancelamentoGeral { get; set; }
		public string CpfResponsavel { get; set; }
		public string NomeResponsavel { get; set; }
		public DateTime? DataGeracao { get; set; }
		public long? IdParecerComplementar { get; set; }
		public string IdParecerComplementar2 { get; set; }
		#endregion

		#region Complemento
		public string DataStatusFormatada { get; set; }
		public string DataValidadeFormatada { get; set; }
		public string NumeroControleString { get; set; }
		public string NumeroProcessoString { get; set; }
		public string NumeroPlanoString { get; set; }
		public string TipoModalidadeDescricao { get; set; }
		public string TipoStatusDescricao { get; set; }
		public string QuantidadeTotalProdutoFormatado { get; set; }
		public string QuantidadeProdutosFormatado { get; set; }
		public string ValorTaxaCambialFormatado { get; set; }
		public string ValorIndiceNacionalizacaoFormatado { get; set; }
		public string ValorIndiceImportacaoFormatado { get; set; }
		public string ValorInsumoNacionalFormatado { get; set; }
		public string ValorInsumoImportadoFobFormatado { get; set; }
		public string ValorInsumoImportacoCfrFormatado { get; set; }
		public string ValorInsumoImportadoRealFormatado { get; set; }
		public string ValorTotalInsumosRealFormatado { get; set; }
		public string ValorExportacaoAprovadoFormatado { get; set; }
		public string QuantidadeExportacaoAprovadoFormatado { get; set; }
		public string InscricaoCadastral { get; set; }
		public string DataCancelamento { get; set; }
		public string DataAlteracaoFormatada { get; set; }
		public string FundamentacaoLegal { get; set; }
		public string ExportacaoComprovadaFormatado { get; set; }
		public string InsumosNacionaisAdquiridosFormatado { get; set; }
		public string InsumosImportadosAutorizadosFormatado { get; set; }
		public string TotalInsumosImportadosInternadosFormatado { get; set; }
		public string NumeroAnoSolicitacaoFormatado { get; set; }

		public string Conclusão { get; set; }
		public string AssinaturaResponsavel { get; set; }
		public string ValorImportadoAutorizadoFormatado { get; set; }
		public string ValorImportadoAcrescimoFormatado { get; set; }
		public string ValorImportadoFreteFormatado { get; set; }
		public string ValorImportadoFormatado { get; set; }
		public string ValorImportadoInternadoFormatado { get; set; }
		public string ValorNacionalAdquiridoFormatado { get; set; }
		public string ValorExportacaoRealizadaFormatado { get; set; }
		public string QuantidadeExportadaUnidadeFormatado { get; set; }
		public string ValorAutorizadoInternadoFormatado { get; set; }
		public string ValorAprovadoAutorizadoFormatado { get; set; }
		public string ValorCancelamentoGeralFormatado { get; set; }

		#endregion
	}
}
