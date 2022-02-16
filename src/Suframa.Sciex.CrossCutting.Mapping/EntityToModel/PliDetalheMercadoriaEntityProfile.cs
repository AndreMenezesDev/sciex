using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class PliDetalheMercadoriaEntityProfile : Profile
	{
		public PliDetalheMercadoriaEntityProfile()
		{
			CreateMap<PliDetalheMercadoriaEntity, PliDetalheMercadoriaVM>()
				.ForMember(dest => dest.ValorUnitarioCondicaoVendaFormatada, opt => opt.MapFrom(src => src.ValorUnitarioCondicaoVenda.HasValue ? string.Format("{0:n7}", src.ValorUnitarioCondicaoVenda) : "0,0000000"))
				.ForMember(dest => dest.ValorUnitarioCondicaoVendaDolarFormatada, opt => opt.MapFrom(src => src.ValorUnitarioCondicaoVendaDolar.HasValue ? string.Format("{0:n7}", src.ValorUnitarioCondicaoVendaDolar) : "0,0000000"))
				.ForMember(dest => dest.ValorTotalCondicaoVendaDolarFormatada, opt => opt.MapFrom(src => src.ValorTotalCondicaoVendaDolar.HasValue ? string.Format("{0:n7}", src.ValorTotalCondicaoVendaDolar) : "0,0000000"))
				.ForMember(dest => dest.QuantidadeComercializadaFormatada, opt => opt.MapFrom(src => src.QuantidadeComercializada.HasValue ? string.Format("{0:n5}", src.QuantidadeComercializada) : "0,00000"))
				.ForMember(dest => dest.IdPli, opt => opt.MapFrom(src => src.PliMercadoria.Pli.IdPLI))
				.ForMember(dest => dest.CodigoDetalheMercadoriaFormatado, opt => opt.MapFrom(src => src.CodigoDetalheMercadoria.Value.ToString("D4")));


		}
	}
}
