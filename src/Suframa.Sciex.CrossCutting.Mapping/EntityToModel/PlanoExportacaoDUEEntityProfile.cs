using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class PlanoExportacaoDUEEntityProfile : Profile
	{
		public PlanoExportacaoDUEEntityProfile()
		{
			CreateMap<PlanoExportacaoDUEEntity, PlanoExportacaoDUEVM>()
				.ForMember(dest => dest.IdDue, opt => opt.MapFrom(src => src.IdDue))
				.ForMember(dest => dest.IdPEProdutoPais, opt => opt.MapFrom(src => src.IdPEProdutoPais))
				.ForMember(dest => dest.CodigoPais, opt => opt.MapFrom(src => src.CodigoPais))
				.ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero))
				.ForMember(dest => dest.DataAverbacao, opt => opt.MapFrom(src => src.DataAverbacao))
				.ForMember(dest => dest.ValorDolar, opt => opt.MapFrom(src => src.ValorDolar))
				.ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.Quantidade))
				.ForMember(dest => dest.SituacaoAnalise, opt => opt.MapFrom(src => src.SituacaoAnalise))
				.ForMember(dest => dest.DescricaoJustificativa, opt => opt.MapFrom(src => src.DescricaoJustificativa))
				.ForMember(dest => dest.PEProdutoPais, opt => opt.Ignore())
				.ForMember(dest => dest.SortManny, opt => opt.Ignore())
				.ForMember(dest => dest.Sort, opt => opt.Ignore())
				.ForMember(dest => dest.Size, opt => opt.Ignore())
				.ForMember(dest => dest.Reverse, opt => opt.Ignore())
				.ForMember(dest => dest.Page, opt => opt.Ignore());
				
		}
	}
}
