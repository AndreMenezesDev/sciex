using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
	public class NaladiVMProfile : Profile
	{
		public NaladiVMProfile()
		{
			CreateMap<NaladiVM, NaladiEntity>()
				.ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
				.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => string.Join("", src.Descricao)));
		}
	}
}
