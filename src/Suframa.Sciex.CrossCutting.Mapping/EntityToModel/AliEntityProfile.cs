using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class AliEntityProfile : Profile
	{
		public AliEntityProfile()
		{
			CreateMap<AliEntity, AliVM>()
				.ForMember(dest => dest.DescricaoStatus, opt => opt.MapFrom(src => ((EnumAliStatus)src.Status).ToString().Replace("_", " ")))
				.ForMember(dest => dest.IdPli, opt => opt.MapFrom(src => src.PliMercadoria.IdPLI))
				.ForMember(dest => dest.NumeroPliConcatenado, opt => opt.MapFrom(src => src.PliMercadoria.Pli.Ano + "/" + src.PliMercadoria.Pli.NumeroPli.ToString("d6")))
				.ForMember(dest => dest.NumeroLi, opt => opt.MapFrom(src => src.PliMercadoria.Li == null ? "0" : src.PliMercadoria.Li.NumeroLi.HasValue ? src.PliMercadoria.Li.NumeroLi.Value.ToString("D10") : "0"))
				.ForMember(dest => dest.NomenclaturaComumMercosul, opt => opt.MapFrom(src => src.PliMercadoria.CodigoNCMMercadoria))
				.ForMember(dest => dest.CodigoProduto, opt => opt.MapFrom(src => src.PliMercadoria.CodigoProduto.HasValue ? src.PliMercadoria.CodigoProduto.Value.ToString("D4") : ""))
				.ForMember(dest => dest.TipoProduto, opt => opt.MapFrom(src => src.PliMercadoria.CodigoTipoProduto.HasValue ? src.PliMercadoria.CodigoTipoProduto.Value.ToString("D3") : ""))
				.ForMember(dest => dest.CodigoModeloProduto, opt => opt.MapFrom(src => src.PliMercadoria.CodigoModeloProduto.HasValue ? src.PliMercadoria.CodigoModeloProduto.Value.ToString("D4") : ""))
				.ForMember(dest => dest.QuantidadeErroAli, opt => opt.MapFrom(src => src.PliMercadoria.Pli.ErroProcessamento.Count))
				;
		}
	}
}
