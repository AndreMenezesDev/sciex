﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PRCInsumoTableColunsVM
	{
		//ESSA VM DEVE CONTER APENAS AS COLUNAS DA ENTITDADE PRCInsumoEntity!!!
		public int IdInsumo { get; set; }
		public int IdPrcProduto { get; set; }
		public int? CodigoInsumo { get; set; }
		public int? CodigoUnidade { get; set; }
		public string TipoInsumo { get; set; }
    	public string CodigoNCM { get; set; }
		public decimal? ValorPercentualPerda { get; set; }
		public int? CodigoDetalhe { get; set; }
		public string DescricaoInsumo { get; set; }
		public string DescricaoPartNumber { get; set; }
		public string DescricaoEspecificacaoTecnica { get; set; }
		public decimal? ValorCoeficienteTecnico { get; set; }
		public decimal? ValorDolarAprovado { get; set; }
		public decimal? QuantidadeAprovado { get; set; }
		public decimal? ValorNacionalAprovado { get; set; }
		public decimal? ValorDolarFOBAprovado { get; set; }
		public decimal? ValorDolarCFRAprovado { get; set; }
		public decimal? ValorFreteAprovado { get; set; }
		public decimal? ValorDolarComp { get; set; }
		public decimal? QuantidadeComp { get; set; }
		public decimal? ValorDolarSaldo { get; set; }
		public decimal? QuantidadeSaldo { get; set; }
		public int? StatusInsumo { get; set; }
		public int? StatusInsumoNovo { get; set; }
		public decimal? QuantidadeAdicional { get; set; }
		public decimal? ValorAdicional { get; set; }
		public decimal? ValorAdicionalFrete { get; set; }
	}

	public class PRCInsumoTableColunsVMCamposComplementares
	{
		public decimal? ValorTotalFOB { get; set; }
		public decimal? ValorTotalCFR { get; set; }
		public decimal? ValorAprovado { get; set; }
	}

}
