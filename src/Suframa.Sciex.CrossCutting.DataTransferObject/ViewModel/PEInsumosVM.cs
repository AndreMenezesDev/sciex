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
	public class PEInsumoVM
	{
		public PEProdutoVM PEProduto { get; set; }
		public List<PEDetalheInsumoVM> ListaPEDetalheInsumo { get; set; }
		public int IdPEInsumo { get; set; }
		public int? IdPEProduto { get; set; }
		public int CodigoInsumo { get; set; }
		public int? CodigoUnidade { get; set; }
		public string DescricaoCodigoUnidade { get; set; }
		public string DescCodigoUnidade { get; set; }
		public string TipoInsumo { get; set; }
		public string DescricaoTipoInsumo { get; set; }
		public string CodigoNcm { get; set; }
		public decimal? ValorPercentualPerda { get; set; }
		public int? CodigoDetalhe { get; set; }
		public string DescricaoInsumo { get; set; }
		public string DescricaoPartNumber { get; set; }
		public string DescricaoEspecificacaoTecnica { get; set; }
		public decimal? ValorCoeficienteTecnico { get; set; }
		public decimal? ValorDolar { get; set; }
		public decimal? QtdSomatorioDetalheInsumo { get; set; }
		public string QtdSomatorioDetalheInsumoFormatada { get; set; }
		public decimal? ValorInsumo { get; set; } = 0;
		public string ValorInsumoFormatada { get; set; }
		public int? SituacaoAnalise { get; set; }
		public string SituacaoAnaliseString { get; set; }
		public string DescricaoJustificativaErro { get; set; }

		//Complemento de classe
		public decimal? QtdProduto { get; set; }
		public decimal? QtdMaxInsumo { get; set; }
		public decimal? ValorTotalFOB { get; set; }
		public string ValorTotalFOBFormatado { get; set; }
		public decimal? ValorTotalFrete { get; set; }
		public string ValorTotalFreteFormatado { get; set; }
		public decimal? ValorTotalCFR { get; set; }
		public string ValorTotalCFRFormatado { get; set; }
		public decimal? ValorTotalInsumo { get; set; }
		public string ValorTotalInsumoFormatado { get; set; }
		public bool IsPlanoEmElaboracao { get; set; }
	}

}
