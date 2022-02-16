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
	public class PEDetalheInsumoVM
	{
		public PEInsumoVM PEInsumo { get; set; }
		public virtual MoedaVM Moeda { get; set; }
		public int IdPEDetalheInsumo { get; set; }
		public int? IdPEInsumo { get; set; }
		public int? IdMoeda { get; set; }
		public int NumeroSequencial { get; set; }
		public int CodigoPais { get; set; }
		public decimal Quantidade { get; set; }
		public string QuantidadeFormatada { get; set; }
		public decimal ValorUnitario { get; set; }
		public string ValorUnitarioFormatada { get; set; }
		public decimal? ValorFrete { get; set; }
		public string ValorFreteFormatada { get; set; }
		public decimal? ValorDolar { get; set; }
		public decimal? ValorDolarFOB { get; set; }
		public string ValorDolarFOBFormatada { get; set; }
		public decimal? ValorDolarCRF { get; set; }
		public string ValorDolarCRFFormatada { get; set; }
		public decimal? ValorInsumo { get; set; }
		public string ValorInsumoFormatada { get; set; }
		public int? SituacaoAnalise { get; set; }
		public string SituacaoAnaliseString { get; set; }
		public string DescricaoJustificativaErro { get; set; }
		public decimal? ValorTotal { get; set; }

	}

	public class PEDetalheInsumoImportadoVM : PEDetalheInsumoVM
	{
		public string DescricaoPais { get; set; }
		public short? CodigoMoeda { get; set; }
		public string DescricaoMoeda { get; set; }

	}

	public class DadosDetalhesInsumosVM
	{
		public List<PEDetalheInsumoImportadoVM> listaDetalhesInsumos { get; set; }
		public decimal? QtdTotalInsumos { get; set; } = 0;
		public string QtdTotalInsumosFormatada { get; set; } = "0";
		public decimal? ValorTotalInsumos { get; set; } = 0;
		public string ValorTotalInsumosFormatada { get; set; } = "0";
	}

	public class DadosProcessoDetalhesInsumosVM
	{
		public PagedItems<PRCDetalheInsumoVM> listaProcessoDetalhesInsumos { get; set; }
		public decimal? QtdTotalInsumos { get; set; } = 0;
		public string QtdTotalInsumosFormatada { get; set; } = "0";
		public decimal? ValorTotalInsumos { get; set; } = 0;
		public string ValorTotalInsumosFormatada { get; set; } = "0";
	}

	public class SalvarDetalheVM : PagedOptions
	{
		public int? IdPEInsumo { get; set; }
		public int? IdPEProduto { get; set; }
		public int? IdPEDetalheInsumo { get; set; }
		public bool IsQuadroNacional { get; set; }
		public decimal Quantidade { get; set; }
		public decimal ValorUnitario { get; set; }
		public decimal ValorUnitarioFOB { get; set; }
		public decimal ValorFrete { get; set; }
		public int CodigoPais { get; set; }
		public int IdMoeda { get; set; }
		public decimal Qtd { get; set; }
		public string DescricaoPartNumber { get; set; }
		public decimal ValorPercentualPerda { get; set; }
		public decimal QtdMaxima { get; set; } = 0;
		public bool FlagPermiteSalvarDadosInsumos { get; set; }
		public decimal ValoresTotais { get; set; }
	}


	public class SalvarDetalhePRCInsumoVM : PagedOptions
	{
		public int? IdPRCInsumo { get; set; }
		public string DescricaoPartNumber { get; set; }
		public decimal ValorPercentualPerda { get; set; }

		public int? IdPEProduto { get; set; }
		public int? IdPEDetalheInsumo { get; set; }
		public bool IsQuadroNacional { get; set; }
		public decimal Quantidade { get; set; }
		public decimal ValorUnitario { get; set; }
		public decimal ValorUnitarioFOB { get; set; }
		public decimal ValorFrete { get; set; }
		public int CodigoPais { get; set; }
		public int IdMoeda { get; set; }
		public decimal Qtd { get; set; }
		public decimal QtdMaxima { get; set; } = 0;
		public bool FlagPermiteSalvarDadosInsumos { get; set; }
		public decimal ValoresTotais { get; set; }
	}

}
