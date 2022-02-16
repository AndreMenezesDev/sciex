using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.DataTransferObjects
{
    public class SetorDtoProfile : Profile
    {
        public SetorDtoProfile()
        {
            CreateMap<SetorDto, SetorEntity>()
               .ForMember(dest => dest.IdSetor, opt => opt.MapFrom(src => src.IdSetor))
               .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
               .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo.HasValue ? (int?)src.Tipo : null))
               .ForMember(dest => dest.DescricaoObservacao, opt => opt.MapFrom(src => src.Observacao))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
               .ForMember(dest => dest.DataAlteracao, opt => opt.MapFrom(src => src.DataAlteracao))
               .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.DataInclusao));
        }
    }
}