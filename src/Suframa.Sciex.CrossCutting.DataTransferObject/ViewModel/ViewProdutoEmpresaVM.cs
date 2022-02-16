using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ViewProdutoEmpresaVM : PagedOptions
	{

		public int IdProdutoEmpresa { get; set; }
		public string CodigoProdutoMontado { get; set; }
		public short CodigoProduto { get; set; }
		public short CodigoTipoProduto { get; set; }
		public short CodigoModeloProduto { get; set; }
		public string CodigoProdutoZFM { get; set; }
		public string Descricao { get; set; }
		public string Cnpj { get; set; }
		public int InscricaoCadastral { get; set; }
		public int CodigoUltilizacao { get; set; }
		public int UME { get; set; }
		public byte CRII { get; set; }
		public string DescricaoCRII { get; set; }

		//complemento de classe
		public int? Id { get; set; }
	}
}
