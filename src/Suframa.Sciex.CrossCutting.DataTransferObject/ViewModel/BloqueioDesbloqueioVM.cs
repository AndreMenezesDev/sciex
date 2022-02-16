using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class BloqueioDesbloqueioVM
	{
		public int? id { get; set; }
		public string cnpjCpf { get; set; }
		public int? inscSuf { get; set; }
		public int? codBloq { get; set; }
		public DateTime dtBloqDesbloq { get; set; }
		public string usuLogin { get; set; }
		public string usuNome { get; set; }
		public string motivo { get; set; }
		public int? tpOperacao { get; set; }
		public int? staMigCd { get; set; }
	}
}