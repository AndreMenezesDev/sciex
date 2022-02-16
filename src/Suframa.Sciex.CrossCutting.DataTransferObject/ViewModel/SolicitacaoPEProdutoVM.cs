using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class SolicitacaoPEProdutoVM : PagedOptions
	{
		public int Id { get; set; }
		public int LoteId { get; set; }
		public int CodigoProdutoPexPam { get; set; }
		public string CodigoProdutoSuframa { get; set; }
		public string CodigoNCM { get; set; }
		public string CodigoTipoProduto { get; set; }
		public string DescricaoModelo { get; set; }
		public decimal? Quantidade { get; set; }
		public decimal? ValorDolar { get; set; }
		public decimal? ValorNacional { get; set; }
		public string CodigoUnidade { get; set; }
		public int? NumeroLote { get; set; }
		public int? AnoLote { get; set; }
		public string InscricaoCadastral { get; set; }
		public int? SituacaoValidacao { get; set; }


		//Campos Complementares
		public string SituacaoValidacaoDescricao { get; set; }
		public string QuantidadeFormatado { get; set; }
		public string ValorDolarFormatado { get; set; }
		public string DataValidacao { get; set; }
		public int QtdErros { get; set; }
		public string DescricaoUnidade { get; set; }
		public long IdEstruturaPropria { get; set; }

	}
}
