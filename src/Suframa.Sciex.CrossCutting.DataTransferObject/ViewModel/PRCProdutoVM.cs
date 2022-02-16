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
	public class PRCProdutoVM : PagedOptions
	{
		public virtual ProcessoExportacaoVM Processo { get; set; }
		public virtual IList<PRCInsumoVM> ListaInsumos { get; set; }
		public virtual IList<PRCProdutoPaisVM> ListaProdutoPais { get; set; }
		public virtual PagedItems<PRCProdutoPaisVM> ListaProdutoPaisPaginada { get; set; }

		public int IdProduto { get; set; }
		public int IdProcesso { get; set; }
		public int? CodigoProdutoExportacao { get; set; }
		public int? CodigoProdutoSuframa { get; set; }
		public string CodigoNCM { get; set; }
		public string DescricaoNCM { get; set; }
		public int? TipoProduto { get; set; }
		public string DescricaoTipoProduto { get; set; }
		public string DescricaoProduto { get; set; }
		public string DescricaoModelo { get; set; }
		public decimal? QuantidadeAprovado { get; set; }
		public int? CodigoUnidade { get; set; }
		public decimal? ValorDolarAprovado { get; set; }
		public decimal? ValorFluxoCaixa { get; set; }
		public decimal? QuantidadeComprovado { get; set; }
		public decimal? ValorDolarComprovado { get; set; }
		public decimal? ValorNacionalComprovado { get; set; }
		public string DescricaoUnidadeMedida { get; set; }

		public string DescCodigoProdutoSuframa { get; set; }
		public string DescCodigoTipoProduto { get; set; }
		public string DescCodigoUnidade { get; set; }

		public string CNPJ { get; set; }
		public bool ExisteSolicAlteracaoEmAnalise { get; set; }
		

	}


}
