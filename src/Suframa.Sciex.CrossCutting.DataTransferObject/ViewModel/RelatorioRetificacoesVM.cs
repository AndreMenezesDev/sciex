using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class RelatorioRetificacoesVM
	{
		public string NumeroLiRetificador { get; set; }
		public List<CampoRetificacaoVM> Lista { get; set;}

	}

	public class CampoRetificacaoVM
	{
		public string Campo { get; set; }
		public string ValorAntigo { get; set; }
		public string ValorNovo { get; set; }
	}
}
