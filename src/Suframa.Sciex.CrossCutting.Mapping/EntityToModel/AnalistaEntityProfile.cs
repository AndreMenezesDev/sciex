using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
	public class AnalistaEntityProfile : Profile
	{
		public AnalistaEntityProfile()
		{
			CreateMap<AnalistaEntity, AnalistaVM>()
				.ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.CPF.CnpjCpfFormat()));
		}
	}
}