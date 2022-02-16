using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DiVM : PagedOptions
	{
		public long? IdDi { get; set; }
		public long NumeroDi { get; set; }
		public int QtdAdicao { get; set; }
		public DateTime? DataDesembaraco { get; set; }
		public DateTime? DataRegistro { get; set; }
		public string DataDesembaracoFormatada { get; set; }
		public string DataRegistroFormatada { get; set; }
		public int TipoDeclaracao { get; set; }
		public string TipoDeclaracaoDescricao { get; set; }
		public string Cnpj { get; set; }
		public int? LocalAlfandega { get; set; }
		public string DescricaoLocalAlfandega { get; set; }
		public int ViaTransporteCodigo { get; set; }
		public string ViaTransporteDescricao { get; set; }
		public decimal TipoMultimodal { get; set; }
		public string TipoMultimodalFormatado { get; set; }
		public string ValorTotalDolar { get; set; }
		public int SetorArmazenamento { get; set; }
		public string SetorArmazenamentoDescricao { get; set; }
		public string ValorTotalMn { get; set; }
		public DateTime? DataProcessamento { get; set; }
		public string DataProcessamentoFormatada { get; set; }
		public int IdUrfEntrada { get; set; }
		public int IdUrfDespacho { get; set; }
		public string UrfEntradaFormatado { get; set; }
		public string UrfDespachoFormatado { get; set; }
		public List<DiArmazemVM> ListaArmazems { get; set; }
		public List<DiEmbalagemVM> ListaEmbalagems { get; set; }
		public List<DiLiVM> ListaDiLis { get; set; }

		#region extensao de classe
		public int IdPliAplicacao { get; set; }
		#endregion
	}
}