using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ResumoVM
	{
		public EnumAcaoCampoSistema Acao { get; set; }
		public string Campo { get; set; }
		public string CampoObjeto { get; set; }
		public string CampoTela { get; set; }
		public DateTime? DataSolicitacao { get; set; }
		public string Descricao { get; set; }
		public int IdCampoSistema { get; set; }
		public int? IdPedidoCorrecao { get; set; }
		public bool IsAlterado { get; set; }
		public string Justificativa { get; set; }
		public string Secao { get; set; }
		public EnumStatusPedidoCorrecao Status { get; set; }
		public string ValorDe { get; set; }
		public string ValorPara { get; set; }
	}
}