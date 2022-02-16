using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class DiLiEntityProfile : Profile
	{
		public DiLiEntityProfile()
		{
			CreateMap<DiLiEntity, DiLiVM>()
			.ForMember(dest => dest.FundamentoLegal, opt => opt.MapFrom(src => src.FundamentoLegal))
			.ForMember(dest => dest.FundamentoLegalDescricao, opt => opt.MapFrom(src => src.FundamentoLegal.Codigo + " | " + src.FundamentoLegal.Descricao))
			.ForMember(dest => dest.MoedaFreteDescricao, opt => opt.MapFrom(src => src.MoedaFrete.CodigoMoeda + " | " + src.MoedaFrete.Descricao))
			.ForMember(dest => dest.MoedaSeguroDescricao, opt => opt.MapFrom(src => src.MoedaSeguro.CodigoMoeda + " | " + src.MoedaSeguro.Descricao))
			.ForMember(dest => dest.MoedaFreteDescricao, opt => opt.MapFrom(src => src.MoedaFrete.CodigoMoeda + " | " + src.MoedaFrete.Descricao))
			.ForMember(dest => dest.TipoMultimodalFormatado, opt => opt.MapFrom(src => src.TipoMultimodal == 0 ? "Não" : "Sim"))
			.ForMember(dest => dest.ValorFreteFormatado, opt => opt.MapFrom(src => String.Format("{0:n2}", src.ValorFreteDolar)))
			.ForMember(dest => dest.ValorFreteDolarFormatado, opt => opt.MapFrom(src => String.Format("{0:n2}", src.ValorFreteDolar)))
			.ForMember(dest => dest.ValorFreteMoedaNegociadaFormatado, opt => opt.MapFrom(src => String.Format("{0:n2}", src.ValorFreteMoedaNegociada)))
			.ForMember(dest => dest.ValorSeguroFormatado, opt => opt.MapFrom(src => String.Format("{0:n2}", src.ValorSeguro)))
			.ForMember(dest => dest.ValorSeguroDolarFormatado, opt => opt.MapFrom(src => String.Format("{0:n2}", src.ValorSeguroDolar)))
			.ForMember(dest => dest.ValorSeguroMoedaNegociadaFormatado, opt => opt.MapFrom(src => String.Format("{0:n2}", src.ValorSeguroMoedaNegociada)))
			.ForMember(dest => dest.ValorMercadoriaDolarFormatado, opt => opt.MapFrom(src => String.Format("{0:n2}", src.ValorMercadoriaDolar)))
			.ForMember(dest => dest.ValorMercadoriaMoedaNegociadaFormatado, opt => opt.MapFrom(src => String.Format("{0:n2}", src.ValorMercadoriaMoedaNegociada)))
			.ForMember(dest => dest.ValorPesoLiquidoFormatado, opt => opt.MapFrom(src => String.Format("{0:n5}", src.ValorPesoLiquido)))
			;
		}
	}
}
