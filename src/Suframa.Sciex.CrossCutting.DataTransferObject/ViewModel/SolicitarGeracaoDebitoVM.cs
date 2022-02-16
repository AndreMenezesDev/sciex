using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class SolicitarGeracaoDebitoVM
    {
		public Servico Servico { get; set; }
		public long NumeroProtocoloEnvio { get; set; }
		public int AnoCorrente { get; set; }
		public string NumeroPLI { get; set; }
		public long InscricaoSuframa { get; set; }
		public string CNPJCPFImportador { get; set; }
		public string RazaoSocialImportador { get; set; }
		public long CobrarDebito { get; set; }
		public long Localidade { get; set; }
		public decimal ValorDebito { get; set; }
		public decimal ValorTcifPli { get; set; }
		public decimal ValorTcifItens { get; set; }
		public decimal ValorReducao { get; set; }
		public ExtratoEstrangeiro ExtratoEstrangeiro { get; set; }
		public string requestBody { get; set; }		
		public int TipoDebitoRetorno { get; set; }
		public EnumStatusTaxaServico StatusDePara { get; set; }
		public string MensagemErro { get; set; }
		public List<DebitoGerado> Debito { get; set; }
	}

	public class Servico
	{
		public short Codigo { get; set; }
	}



	public class ExtratoEstrangeiro
	{
		public string NumeroPLI { get; set; }
		public decimal ValorMercadoriaEmReal { get; set; }
		public decimal ValorTCIFBasePLI { get; set; }
		public decimal LimitadorPLI { get; set; }
		public decimal ValorTCIFRealPLI { get; set; }
		public string DataCotacaoDolar { get; set; }
		public decimal TaxaDolar { get; set; }		
		public List<ListaNCM> ListaNCM { get; set; }
	}

	public class ListaNCM
	{
		public long NumeroNCM { get; set; }
		public decimal ValorMercadoriaMoedaNegociada { get; set; }
		public decimal ValorMercadoriaEmReal { get; set; }
		public short MoedaCambio { get; set; }
		public string DataCambio { get; set; }
		public decimal TaxaCambio { get; set; }
		public long QtdItens { get; set; }
		public List<Itens> Itens { get; set; }
	}


	public class Itens
	{
		public string Numero { get; set; }
		public string NomeItem { get; set; }
		public decimal ValorItemEmReal { get; set; }
		public decimal ValorTCIFBaseItem { get; set; }
		public decimal LimitadorItem { get; set; }
		public decimal ValorTCIFRealItem { get; set; }
		public string Isencao { get; set; }
		public decimal Reducao { get; set; }

	}

	public class DebitoGerado
	{
		public int Numero { get; set; }
		public int AnoDebito { get; set; }
		public string DataVencimento { get; set; }
		public int TipoDebitoRetorno { get; set; }
		public string MensagemErro { get; set; }
		public string Mensagem { get; set; }
		public decimal? ValorCapa { get; set; }
		public decimal? ValorItens { get; set; }


	}




}