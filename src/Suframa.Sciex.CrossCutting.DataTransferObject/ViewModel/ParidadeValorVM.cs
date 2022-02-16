using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ParidadeValorVM
	{
		public int? IdParidadeValor { get; set; }
		public int? IdParidadeCambial { get; set; }
		public int? IdMoeda { get; set; }
		public decimal Valor { get; set; }
	}
}