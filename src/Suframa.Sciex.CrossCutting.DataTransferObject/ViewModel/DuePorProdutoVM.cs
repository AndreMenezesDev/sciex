using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DuePorProdutoVM
	{
		/*public IEnumerable<UsuarioInternoVM> Analistas { get; set; }
		public int? Ano { get; set; }
		public IEnumerable<CalendarioVM> Calendario { get; set; }
		public DateTime? DataAtendimento { get; set; }
		public IEnumerable<CalendarioDiaVM> Dias { get; set; }
		public IEnumerable<CalendarioHoraVM> Horarios { get; set; }
		public int? IdCalendarioAgendamento { get; set; }
		public int? IdUnidadeCadastradora { get; set; }
		public int? IdUsuarioInterno { get; set; }
		public bool IsMesInteiro { get; set; }
		public int? Mes { get; set; }
		public string UnidadeCadastradora { get; set; }*/
		public int? IdDue { get; set; }
		public int IdPEProdutoPais { get; set; }
		public int IdPEProduto{ get; set; }
		public string Numero { get; set; }
		public decimal ValorDolar { get; set; }
		public DateTime DataAverbacao { get; set; }
		public decimal Quantidade { get; set; }
		public int CodigoPais { get; set; }
		public int CodigoProduto { get; set; }

		public bool Sucesso { get; set; }
		public string RetornoString { get; set; }
	}
}