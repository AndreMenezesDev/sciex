using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PEProdutoPaisVM : PagedOptions
	{
		public int IdPEProdutoPais { get; set; }
		public int? IdPEProduto { get; set; }
		public decimal Quantidade { get; set; }
		public decimal ValorDolar { get; set; }
		public int CodigoPais { get; set; }
		public int? SituacaoAnalise { get; set; }
		public string DescricaoJustificativaErro { get; set; }
		// complemento de classe
		public string DescricaoPais { get; set; }
		public string Mensagem { get; set; }
		public string QuantidadeFormatado { get; set; }
		public string ValorDolarFormatado { get; set; }
	}
	public class PEProdutoPaisComplementoVM : PEProdutoPaisVM
	{
		public List<PlanoExportacaoDUEComplementoVM> ListaPEDue { get; set; }
	}
}
