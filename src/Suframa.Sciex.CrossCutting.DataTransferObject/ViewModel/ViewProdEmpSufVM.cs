using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ViewProdEmpSufVM : PagedOptions
	{

		public int IdProdutoEmpresaExportacao { get; set; }
		public int IdCodigoProduto { get; set; }
		public short CodigoProduto { get; set; }
		public int IdCodigoProdutoSuframa { get; set; }
		public int CodigoProdutoSuframa { get; set; }
		public string DescricaoProduto { get; set; }
		public int IdCodigoTipoProduto { get; set; }
		public short CodigoTipoProduto { get; set; }
		public string DescricaoTipoProduto { get; set; }
		public string Cnpj { get; set; }
		public int InscricaoCadastral { get; set; }
		public int IdUnidadeMedida { get; set; }
		public int IdCodigoNCM { get; set; }
		public string CodigoNCM { get; set; }
		public string DescricaoNCM { get; set; }
		public short CodigoModelo { get; set; }
		public short DescricaoModelo { get; set; }

		//complemento de classe
		public int? Id { get; set; }
		public string Descricao { get; set; }
	}
}
