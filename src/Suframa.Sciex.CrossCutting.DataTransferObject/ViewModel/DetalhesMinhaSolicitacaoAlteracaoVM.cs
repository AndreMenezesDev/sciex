using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DetalhesMinhaSolicitacaoAlteracaoVM
	{
		public int IdSolicitacaoAlteracao { get; set; }
		public string NumeroProcesso { get; set; }
		public string NumeroSolicitacao { get; set; }
		public string Modalidade { get; set; }
		public string DescricaoStatus { get; set; }
		public int? NumeroProduto { get; set; }
		public string DescricaoProduto { get; set; }
		public string DescricaoTipo { get; set; }
		public string Modelo { get; set; }
		public string Ncm { get; set; }
		public string QtdTotal { get; set; }
		public string ValorTotal { get; set; }
		public string Unidade { get; set; }
	}
}
