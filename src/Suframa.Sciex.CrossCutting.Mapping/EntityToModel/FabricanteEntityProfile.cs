using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class FabricanteEntityProfile : Profile
	{
		public FabricanteEntityProfile()
		{
			CreateMap<FabricanteEntity, FabricanteVM>()
				.ForMember(dest => dest.CodigoDescricaoPaisConcatenado, opt => opt.MapFrom(src => src.CodigoPais + " | " + src.DescricaoPais));
		}
	}
}