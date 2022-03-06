using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DuePorProdutoVM
	{
		public int? IdDue { get; set; }
		public int IdPEProdutoPais { get; set; }
		public int IdPEProduto{ get; set; }
		public string Numero { get; set; }
		public string DescricaoJustificativa { get; set; }
		public decimal ValorDolar { get; set; }
		public DateTime DataAverbacao { get; set; }
		public decimal Quantidade { get; set; }
		public int CodigoPais { get; set; }
		public int CodigoProduto { get; set; }

		public bool Sucesso { get; set; }
		public string RetornoString { get; set; }
	}
}