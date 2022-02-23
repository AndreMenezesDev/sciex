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
	public class ParecerTecnicoProdutoVM : PagedOptions
	{
		#region CampoTabela
		public long IdParecerTecnicoProduto { get; set; }
		public int? NumeroSequencia { get; set; }
		public int CodigoProdutoExportacao { get; set; }
		public int? CodigoProdutoSuframa { get; set; }
		public string CodigoProdutoSuframaFormatado { get; set; }
		public string CodigoNCM { get; set; }
		public int? CodigoTipoProduto { get; set; }
		public string DescricaoTipo { get; set; }
		public string DescricaoModelo { get; set; }
		public string DescricaoUnidade { get; set; }
		public string DescricaoUnidadeFormatado { get; set; }
		public decimal? ValorUnitarioProdutoAprovado { get; set; }
		public decimal? QuantidadeProdutoAprovado { get; set; }
		public decimal? ValorInsumoNacionalProduto { get; set; }
		public decimal? ValorInsumoImportacaoProduto { get; set; }
		public decimal? ValorInsumoImportacaoProdutoFob { get; set; }
		public decimal? ValorInsumoImportacoCfr { get; set; }
		public string DescricaoPais { get; set; }
		public decimal? QuantidadePais { get; set; }
		public decimal? ValorPais { get; set; }
		public long? IdParecerTecnico { get; set; }
		public decimal? ValorUnitarioProdutoComprovado { get; set; }
		public decimal? ValorInsumoNacionalAdquirido { get; set; }
		public decimal? ValorInsumoImportado { get; set; }
		public decimal? ValorExportacaoComprovado { get; set; }
		public decimal? ValorExportacaoNacionalComprovado { get; set; }
		#endregion

		#region Complemento
		public string ValorUnitarioProdutoComprovadoFormatado { get; set; }
		public string ValorInsumoNacionalAdquiridoFormatado { get; set; }
		public string ValorInsumoImportadoFormatado { get; set; }
		public string ValorExportacaoComprovadoFormatado { get; set; }
		public string ValorExportacaoNacionalComprovadoFormatado { get; set; }
		public string NumeroSequenciaFormatado { get; set; }
		public string ValorUnitarioProdutoAprovadoFormatado { get; set; }
		public string QuantidadeProdutoAprovadoFormatado { get; set; }
		public string ValorInsumoNacionalProdutoFormatado { get; set; }
		public string ValorInsumoImportacaoProdutoFormatado { get; set; }
		public string ValorInsumoImportacaoProdutoFobFormatado { get; set; }
		public string ValorInsumoImportacoCfrFormatado { get; set; }
		public string QuantidadePaisFormatado { get; set; }
		public string ValorPaisFormatado { get; set; }
		public IEnumerable<object> ListaPaisesParaProduto { get; set; }
		#endregion
	}
}
