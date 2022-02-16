using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PliProdutoVM : PagedOptions
	{
		public int? IdPliProduto { get; set; }
		public long IdPLI { get; set; }
		public short CodigoProduto { get; set; }
		public short CodigoTipoProduto { get; set; }
		public short CodigoModeloProduto { get; set; }
		public string Descricao { get; set; }

		public List<PliMercadoriaVM> PliMercadoriaVMList { get; set; }

		// complemento de classe
		public int IdProdutoEmpresa { get; set; }
		public string MensagemErro { get; set; }		
	}
}
