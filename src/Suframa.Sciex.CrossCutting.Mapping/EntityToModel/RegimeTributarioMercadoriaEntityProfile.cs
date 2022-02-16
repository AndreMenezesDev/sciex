using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class RegimeTributarioMercadoriaEntityProfile : Profile
	{
		public RegimeTributarioMercadoriaEntityProfile()
		{
			CreateMap<RegimeTributarioMercadoriaEntity, RegimeTributarioMercadoriaVM>()
				.ForMember(dest => dest.CodigoDescricaoRegimeTributario, opt => opt.MapFrom(src => src.RegimeTributario.Codigo.ToString() + " - " + src.RegimeTributario.Descricao))
				.ForMember(dest => dest.CodigoDescricaoFundamentoLegal, opt => opt.MapFrom(src => src.FundamentoLegal.Codigo.ToString() + " - " + src.FundamentoLegal.Descricao))
				.ForMember(dest => dest.CodigoDescricaoMunicipio, opt => opt.MapFrom(src => src.CodigoMunicipio.ToString() + " - " + src.DescricaoMunicipio.ToString()))
				.ForMember(dest => dest.CodigoRegimeTributario, opt => opt.MapFrom(src => src.RegimeTributario.Codigo))
				.ForMember(dest => dest.CodigoFundamentoLegal, opt => opt.MapFrom(src => src.FundamentoLegal.Codigo))
				.ForMember(dest => dest.DescricaoRegimeTributario, opt => opt.MapFrom(src => src.RegimeTributario.Descricao))
				.ForMember(dest => dest.DescricaoFundamentoLegal, opt => opt.MapFrom(src => src.FundamentoLegal.Descricao))

				.ForMember(dest => dest.DataVigenciaFormatado, opt => opt.MapFrom(src => src.DataInicioVigencia.ToShortDateString()));
		}
	}
}