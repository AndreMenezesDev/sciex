using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class InsumoVM
	{
		public string Descricao { get; set; }
		public int? IdInsumo { get; set; }
		public int? Id { get; set; }
		public int? CodigoInsumo { get; set; }
		public string DescricaoInsumo { get; set; }
		public int CodigoProdutoExportacao { get; set; }
		public string cnpjLogado { get; set; }
	}
}
