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
	public class PlanoExportacaoDUEVM : PagedOptions
	{
		public virtual PEProdutoPaisComplementoVM PEProdutoPais { get; set; }
		public int IdDue { get; set; }
		public int? IdPEProduto { get; set; }
		public int? IdPEProdutoPais { get; set; }
		public int CodigoPais { get; set; }
		public string Numero { get; set; }
		public DateTime DataAverbacao { get; set; }
		public decimal ValorDolar { get; set; }
		public decimal Quantidade { get; set; }
		public int? SituacaoAnalise { get; set; }
		public string DescricaoSituacaoAnalise { get; set; }
		public string DescricaoJustificativa { get; set; }
	}

	public class PlanoExportacaoDUEComplementoVM : PlanoExportacaoDUEVM
	{
		public string DescricaoPais { get; set; }
		public string DataAverbacaoFormatada { get; set; } 

	}

}
