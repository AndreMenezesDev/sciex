using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class UFEntityProfile : Profile
    {
        public UFEntityProfile()
        {
            CreateMap<UFEntity, UFDto>()
                .ForMember(dest => dest.UF, opt => opt.MapFrom(src => src.SiglaUF));
        }
    }
}