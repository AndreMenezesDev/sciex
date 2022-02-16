using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ViewAtividadeEconomicaPrincipalVM : PagedOptions
	{		
		public string CodigoConcla { get; set; }		
		public int Descricao { get; set; }
		public int TipoAtividade { get; set; }
		public bool StatusAtividadeAtuante { get; set; }
		public string DescricaoStatusAtividadeAtuante { get; set; }
		public int IdSetor { get; set; }
		public string DescricaoSetor { get; set; }
		public string CNPJ { get; set; }
	}
}
