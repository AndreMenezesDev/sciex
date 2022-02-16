using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class PlanoExportacaoEntityProfile : Profile
	{
		public PlanoExportacaoEntityProfile()
		{
			CreateMap<PlanoExportacaoEntity, PlanoExportacaoVM>()
				.ForMember(dest => dest.ListaPEProdutos, opt => opt.MapFrom(src => src.ListaPEProdutos))
				.ForMember(dest => dest.ListaAnexos, opt => opt.MapFrom(src => src.ListaAnexos != null ? src.ListaAnexos : null))
				.ForMember(dest => dest.DataCadastroFormatada, opt => opt.Ignore())
				.ForMember(dest => dest.NumeroAnoPlanoFormatado, opt => opt.Ignore())
				.ForMember(dest => dest.TipoExportacaoString, opt => opt.Ignore())
				.ForMember(dest => dest.SituacaoString, opt => opt.Ignore())
				.ForMember(dest => dest.TipoModalidadeString, opt => opt.Ignore());
				
		}
	}
}
