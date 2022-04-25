using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class RelatorioAnalisadorDuesVM : PagedOptions
	{

		public int NumeroPlano;
		public string PlanoStatus;
		public string NumeroPlanoFormated;
		public string DataStatus;
		public string Due;
		public decimal ValorDue;
		public decimal QuantidadeDue;
		public int? AnoProcesso;
		public int NumeroProcesso;
		public string AnoNumPlano;
		public string NomeEmpresa;
		public int NumeroInscriçãoCadastral;

		public RelatoriosDuesVM Relatorios { get; set; }


	}


}