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
	public class ParametrizarAnalistaVM : PagedOptions
	{
		public int IdAnalista { get; set; }
		public string CPF { get; set; }
		public string Nome { get; set; }
		public string SiglaSetor { get; set; }
		public string DescricaoSetor { get; set; }
		public DateTime DataHoraSincronizacao { get; set; }
		public byte? SituacaoVisual { get; set; }
		public byte? SituacaoVisualSetada { get; set; }


		//Complemento de classe
		public string Mensagem { get; set; }

	}

}
