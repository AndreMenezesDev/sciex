using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class TipoDocumentoEntityProfile : Profile
    {
        public TipoDocumentoEntityProfile()
        {
            CreateMap<TipoDocumentoEntity, TipoDocumentoVM>()
                .ForMember(dest => dest.TipoOrigem, opt => opt.MapFrom(src => (EnumTipoOrigemDocumento)src.TipoOrigem))
                .ForMember(dest => dest.IdProtocolo, opt => opt.Ignore())
                .ForMember(dest => dest.IsSelecionado, opt => opt.Ignore());
        }
    }
}