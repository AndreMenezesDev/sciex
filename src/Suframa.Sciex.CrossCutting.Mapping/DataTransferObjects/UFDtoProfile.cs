using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.DataTransferObjects
{
    public class UFDtoProfile : Profile
    {
        public UFDtoProfile()
        {
            CreateMap<UFDto, UFEntity>()
                .ForMember(dest => dest.SiglaUF, opt => opt.MapFrom(src => src.UF));
        }
    }
}