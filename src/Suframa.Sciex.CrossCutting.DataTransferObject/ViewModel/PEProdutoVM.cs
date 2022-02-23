using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PEProdutoVM : PagedOptions
	{
		public int IdPEProduto { get; set; }
		public int CodigoProdutoExportacao { get; set; }
		public int CodigoProdutoSuframa { get; set; }
		public string CodigoNCM { get; set; }
		public int CodigoTipoProduto { get; set; }
		public string DescricaoModelo { get; set; }
		public decimal Qtd { get; set; }
		public decimal ValorDolar { get; set; }
		public decimal ValorFluxoCaixa { get; set; }
		public int IdPlanoExportacao { get; set; }
		public int CodigoUnidade { get; set; }
		public decimal? ValorNacional { get; set; }
		public IList<PEInsumoVM> ListaPEInsumo { get; set; }
		public IList<PEProdutoPaisVM> ListaPEProdutoPais { get; set; }

		// complemento de classe
		public int IdLEProduto { get; set; }
		public string MensagemErro { get; set; }
		public string DescCodigoProdutoSuframa { get; set; }
		public string DescCodigoTipoProduto { get; set; }
		public string DescCodigoUnidade { get; set; }
		public string QtdFormatado { get; set; }
		public string ValorDolarFormatado { get; set; }
	}
	public class PEProdutoComplementoVM : PEProdutoVM
	{
		public IList<PEProdutoPaisComplementoVM> ListaPEProdutoPais { get; set; }
	}
}
