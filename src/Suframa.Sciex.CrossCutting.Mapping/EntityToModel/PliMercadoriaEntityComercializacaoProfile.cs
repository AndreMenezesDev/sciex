using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class PliMercadoriaEntityComercializacaoProfile : Profile
	{
		public PliMercadoriaEntityComercializacaoProfile()
		{
			CreateMap<PliMercadoriaEntity, PliMercadoriaComercializacaoVM>()
				.ForMember(dest => dest.ValorTotalCondicaoVendaDolarFormatado, opt => opt.MapFrom(src => src.ValorTotalCondicaoVendaDolar.HasValue ? string.Format("{0:n2}", src.ValorTotalCondicaoVendaDolar) : "0,00"))
				.ForMember(dest => dest.ValorTotalCondicaoVendaFormatado, opt => opt.MapFrom(src => src.ValorTotalCondicaoVenda.HasValue ? string.Format("{0:n2}", src.ValorTotalCondicaoVenda) : "0,00"))
				.ForMember(dest => dest.ValorTotalCondicaoVendaRealFormatado, opt => opt.MapFrom(src => src.ValorTotalCondicaoVendaReal.HasValue ? string.Format("{0:n2}", src.ValorTotalCondicaoVendaReal) : "0,00"))
				.ForMember(dest => dest.ListaPliDetalheMercadoriaVM, opt => opt.MapFrom(src => src.PliDetalheMercadoria))
				.ForMember(dest => dest.ListaPliProcessoAnuenteVM, opt => opt.MapFrom(src => src.PliProcessoAnuente))
				.ForMember(dest => dest.CodigoMoeda, opt => opt.MapFrom(src => src.Moeda.CodigoMoeda))
				.ForMember(dest => dest.CodigoIncoterms, opt => opt.MapFrom(src => src.Incoterms.Codigo))
				.ForMember(dest => dest.URFNaladi, opt => opt.MapFrom(src => src.Naladi.Codigo))
				.ForMember(dest => dest.URFDespacho, opt => opt.MapFrom(src => src.UnidadeReceitaFederalDespacho.Codigo))
				.ForMember(dest => dest.EntradaMercadoria, opt => opt.MapFrom(src => src.UnidadeReceitaFederalEntrada.Codigo))
				.ForMember(dest => dest.CodigoAladi, opt => opt.MapFrom(src => src.Aladi.Codigo))
				.ForMember(dest => dest.DescricaoAladi, opt => opt.MapFrom(src => src.Aladi.Descricao))
				.ForMember(dest => dest.CodigoRegimeTributario, opt => opt.MapFrom(src => src.RegimeTributario.Codigo))
				.ForMember(dest => dest.DescricaoRegimeTributario, opt => opt.MapFrom(src => src.RegimeTributario.Descricao))
				.ForMember(dest => dest.CodigoFundamentalLegal, opt => opt.MapFrom(src => src.FundamentoLegal.Codigo))
				.ForMember(dest => dest.DescricaoFundamentalLegal, opt => opt.MapFrom(src => src.FundamentoLegal.Descricao))
				.ForMember(dest => dest.InfCodigo, opt => opt.MapFrom(src => src.InstituicaoFinanceira.Codigo != 0 ? src.InstituicaoFinanceira.Codigo.ToString() : ""))
				.ForMember(dest => dest.InfDescricao, opt => opt.MapFrom(src => src.InstituicaoFinanceira.Descricao))
				.ForMember(dest => dest.MopCodigo, opt => opt.MapFrom(src => src.ModalidadePagamento.Codigo))
				.ForMember(dest => dest.MopDescricao, opt => opt.MapFrom(src => src.ModalidadePagamento.Descricao))
				.ForMember(dest => dest.MotCodigo, opt => opt.MapFrom(src => src.Motivo.Codigo != 0 ? src.Motivo.Codigo.ToString() : ""))
				.ForMember(dest => dest.MotDescricao, opt => opt.MapFrom(src => src.Motivo.Descricao))
				.ForMember(dest => dest.TipoAcordoTarifarioDescricao, opt => opt.MapFrom(src => src.TipoAcordoTarifario == null ? "" : ((EnumTipoAcordoTarifario)src.TipoAcordoTarifario).ToString().Replace("_", " ")))
				.ForMember(dest => dest.TipoCOBCambialDescricao, opt => opt.MapFrom(src => src.TipoCOBCambial == null ? "" : ((EnumTipoCoberturaCambial)src.TipoCOBCambial).ToString().Replace("_", " ")))
				.ForMember(dest => dest.DescricaoMoeda, opt => opt.MapFrom(src => src.Moeda.Descricao))
				.ForMember(dest => dest.NumeroALI, opt => opt.MapFrom(src => src.Ali.NumeroAli))
				.ForMember(dest => dest.CNPJ, opt => opt.MapFrom(src => src.Pli.Cnpj))
				.ForMember(dest => dest.RazaoSocial, opt => opt.MapFrom(src => src.Pli.RazaoSocial))
				.ForMember(dest => dest.NumeroPliConcatenado, opt => opt.MapFrom(src => src.Pli.Ano + "/" + src.Pli.NumeroPli.ToString("d6")))
				.ForMember(dest => dest.DataProcessamento, opt => opt.MapFrom(src => src.Pli.PLIHistorico.FirstOrDefault().DataEvento == null ? "" : src.Pli.PLIHistorico.LastOrDefault().DataEvento.Value.ToShortDateString()))
				.ForMember(dest => dest.DescricaoTipoFornecedor, opt => opt.MapFrom(src => src.TipoFornecedor.HasValue ? (src.TipoFornecedor == 1 ? "1 - O FABRICANTE/PRODUTOR É O EXPORTADOR" : src.TipoFornecedor == 2 ? "2 - O FABRICANTE/PRODUTOR NÃO É O EXPORTADOR" : "3 - O FABRICANTE/PRODUTOR É DESCONHECIDO") : "0 - TIPO DE FORNECEDOR NÃO INFORMADO"))
				.ForMember(dest => dest.CodigoDescricaoMoeda, opt => opt.MapFrom(src => (src.Moeda != null ? src.Moeda.CodigoMoeda + " | " + src.Moeda.Descricao : "")))
				.ForMember(dest => dest.TipoAreaBeneficio, opt => opt.MapFrom(src => src.FundamentoLegal.TipoAreaBeneficio))
				.ForMember(dest => dest.CodigoDetalheMercadoria, opt => opt.MapFrom(src => src.PliDetalheMercadoria.FirstOrDefault().CodigoDetalheMercadoria.Value.ToString("D4")))
				.ForMember(dest => dest.TipoALI, opt => opt.MapFrom(src => src.Pli.TipoDocumento == 1 ? "Normal" : "Substitutiva"))
				.ForMember(dest => dest.StatusALI, opt => opt.MapFrom(src => src.Ali.Status))
				.ForMember(dest => dest.DescricaoFornecedor, opt => opt.MapFrom(src => src.Fornecedor.RazaoSocial))
				.ForMember(dest => dest.DataALIFormatada, opt => opt.MapFrom(src => src.Ali.DataCadastro.ToShortDateString()))
				.ForMember(dest => dest.NumeroLI, opt => opt.MapFrom(src => src.Li != null && src.Li.NumeroLi == null ? "-" : src.Li.NumeroLi.Value.ToString("d10")))
				.ForMember(dest => dest.DataGeracaoLI, opt => opt.MapFrom(src => src.Li != null && src.Li.DataGeracaoLI.HasValue ? src.Li.DataGeracaoLI.Value.ToShortDateString() : "-"))
				.ForMember(dest => dest.DataALICanceladaFormatada, opt => opt.MapFrom(src => src.Li != null && src.Li.DataCancelamento.HasValue ? src.Li.DataCancelamento.Value.ToShortDateString() : "-"))
				.ForMember(dest => dest.CodigoDescricaoPaisFornecedorConcatenado, opt => opt.MapFrom(src => src.CodigoPais + " | " + src.DescricaoPais))
				.ForMember(dest => dest.QuantidadeErroAli, opt => opt.MapFrom(src => src.Pli.ErroProcessamento.Count))
				.ForMember(dest => dest.NumeroTransmissaoSERPRO, opt => opt.MapFrom(src => src.Li != null && src.Li.NumeroProtocoloLI != null && src.Li.NumeroProtocoloLI.HasValue ? src.Li.NumeroProtocoloLI.Value.ToString("d10") : "-"))
				.ForMember(dest => dest.DataDIFormatada, opt => opt.MapFrom(src => src.Li != null && src.Li.Di != null ? src.Li.Di.DataCadastro.ToShortDateString() : "-"))
				.ForMember(dest => dest.TipoOrigem, opt => opt.MapFrom(src => src.Pli.TipoOrigem.HasValue ? src.Pli.TipoOrigem.Value : 0))
				.ForMember(dest => dest.PesoLiquidoString, opt => opt.MapFrom(src => src.PesoLiquido.HasValue ? string.Format("{0:n5}", src.PesoLiquido.Value) : "0,00000"))
				.ForMember(dest => dest.QuantidadeEstatisticaString, opt => opt.MapFrom(src => src.QuantidadeUnidadeMedidaEstatistica.HasValue ? string.Format("{0:n5}", src.QuantidadeUnidadeMedidaEstatistica.Value) : "0,00000"));
		}
	}
}
