using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ViewInsumoPadraoVM : PagedOptions
	{
		public int IdInsumoPadrao { get; set; }
		public short CodigoProduto { get; set; }
		public string CodigoNCMMercadoria { get; set; }
		public int CodigoDetalheMercadoria { get; set; }
		public string DescricaoDetalheMercadoria { get; set; }
		public int IdUnidadeMedida { get; set; }
		public int CodigoUnidadeMedida { get; set; }
		public string DescricaoUnidadeMedida { get; set; }
		public string SiglaUnidadeMedida { get; set; }


		// complemento de class
		public int? Id { get; set; }
		public string Descricao { get; set; }

	}

	public class ViewInsumoPadraoDropDown : ViewInsumoPadraoVM
	{
		public string ValorCodigoNCM { get; set; }
		public int ValorCodigoDetalheMercadoria { get; set; }
		public int ValorCodigoProdutoSuframa { get; set; }
	}
}
