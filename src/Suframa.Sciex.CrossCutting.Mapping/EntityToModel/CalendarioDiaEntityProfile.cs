using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class CalendarioDiaEntityProfile : Profile
    {
        public CalendarioDiaEntityProfile()
        {
            CreateMap<CalendarioDiaEntity, CalendarioDiaVM>()
                .ForMember(dest => dest.Horas, opt => opt.MapFrom(src => src.CalendarioHora));
        }
    }
}