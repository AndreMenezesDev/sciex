using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class SetorEntityProfile : Profile
    {
        public SetorEntityProfile()
        {
            CreateMap<SetorEntity, SetorDto>()
                .ForMember(dest => dest.IdSetor, opt => opt.MapFrom(src => src.IdSetor))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => (EnumTipoSetorEmpresarial?)src.Tipo))
                .ForMember(dest => dest.Observacao, opt => opt.MapFrom(src => src.DescricaoObservacao))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.DataAlteracao, opt => opt.MapFrom(src => src.DataAlteracao))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.DataInclusao));

            CreateMap<SetorEntity, ManterSetorEmpresarialVM>()
                .ForMember(dest => dest.IdSetor, opt => opt.MapFrom(src => src.IdSetor))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => (EnumTipoSetorEmpresarial?)src.Tipo))
                .ForMember(dest => dest.Observacao, opt => opt.MapFrom(src => src.DescricaoObservacao))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}