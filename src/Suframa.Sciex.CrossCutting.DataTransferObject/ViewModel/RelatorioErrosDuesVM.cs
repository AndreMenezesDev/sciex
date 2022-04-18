using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class RelatorioErrosDuesVM : RequestResponseVM
	{
		public string NomeEmpresa { get; set; }
		public int? InscricaoCadastral { get; set; }
		public long NumeroPlano { get; set; }
		public string Modalidade { get; set; }
		public string Tipo { get; set; }
		public string DataStatus { get; set; }
		public string DataRecebimento { get; set; }
		public int AnoPlano { get; set; }
		public string AnoNumProcesso { get; set; }
		public string AnoNumPlano { get; set; }
		public string DataImpressao { get; set; }
		public string NumeroPlanoFormated { get; set; }
		public List<RelatorioDuesVM> RelatorioHistoricoAnalise { get; set; }
		public List<RelatorioDuesVM> RelatorioDePara { get; set; }
	}

	public class RelatorioDuesVM
	{
		public int Codigo { get; set; }
		public string NumeroDue { get; set; }
		public string Situacao { get; set; }
		public string Responsavel { get; set; }
		public string Justificativa { get; set; }
		public string PaisDestino { get; set; }
		public string DataAverbacao { get; set; }
		public Decimal Quantidade { get; set; }
		public Decimal Valor { get; set; }
	}

}
