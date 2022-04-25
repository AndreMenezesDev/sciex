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

		public int NumeroPlano { get; set; }
		public string PlanoStatus { get; set; }
		public string NumeroPlanoFormated { get; set; }
		public string DataStatus { get; set; }
		public string Due { get; set; }
		public decimal ValorDue { get; set; }
		public decimal QuantidadeDue { get; set; }
		public int? AnoProcesso { get; set; }
		public int NumeroProcesso { get; set; }
		public string AnoNumPlano { get; set; }
		public string NomeEmpresa { get; set; }
		public int NumeroInscricaoCadastral { get; set; }

		public RelatoriosDuesVM Relatorios { get; set; }


	}


}