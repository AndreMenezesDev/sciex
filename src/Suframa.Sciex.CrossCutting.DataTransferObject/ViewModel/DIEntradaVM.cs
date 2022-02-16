using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DIEntradaVM : PagedOptions
	{
		public long Id { get; set; }
		public string Numero { get; set; }
		public string QuantidadeAdicao { get; set; }
		public string DataDesembaraco { get; set; }
		public string DataRegistro { get; set; }
		public string CodigoDeclaracao { get; set; }
		public string Cnpj { get; set; }
		public string CodigoUrfEntradaCarga { get; set; }
		public string CodigoUrfDespacho { get; set; }
		public string CodigoRecintoAlfandega { get; set; }
		public string CodigoViaTransCarga { get; set; }
		public string TipoMultimodal { get; set; }
		public string ValorTotalMleDolar { get; set; }
		public string CodigoSetorArmazena { get; set; }
		public int? Situacao { get; set; }
		public string ValorTotalMleMn { get; set; }
		public DateTime DataEntrada { get; set; }

		public string Identificador { get; set; }
		public string DataValidacao { get; set; }
		public string StatusValidacao { get; set; }
		public string NomeEmpresa { get; set; }
		public int SituacaoLeitura { get; set; }
		public int QtdErros { get; set; }
	}
}
