using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class PapelEntityProfile : Profile
    {
        public PapelEntityProfile()
        {
            CreateMap<PapelEntity, PapelVM>()
                .ForMember(dest => dest.IdPapel, opt => opt.MapFrom(src => (EnumPapel)src.IdPapel))
            ;
        }
    }
}