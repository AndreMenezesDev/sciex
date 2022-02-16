using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DocumentosComprobatoriosVM
	{
		public string Cpf { get; set; }
		public string CpfSolicitante { get; set; }
		public IEnumerable<DocumentoComprobatorioVM> DocumentosComprobatorios { get; set; }
		public IEnumerable<DocumentoComprobatorioVM> DocumentosComprobatoriosVigentes { get; set; }
		public int? IdPessoaFisica { get; set; }
		public int? IdPessoaJuridica { get; set; }
		public int? IdProtocolo { get; set; }
		public int? IdRequerimento { get; set; }
		public int? IdTipoRequerimento { get; set; }
		public bool IsGerarProtocolo { get; set; }
		public EnumTipoOrigemRequisicao TipoOrigem { get; set; }
	}
}