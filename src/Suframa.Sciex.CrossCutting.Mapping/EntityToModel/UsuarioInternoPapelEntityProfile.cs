using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class UsuarioInternoPapelEntityProfile : Profile
    {
        public UsuarioInternoPapelEntityProfile()
        {
            CreateMap<UsuarioPapelEntity, UsuarioInternoPapelVM>()
                .ForMember(dest => dest.Papel, opt => opt.MapFrom(src => src.Papel.Descricao))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.UsuarioInterno.Nome))
                .ForMember(dest => dest.IdUsuarioInternoPapel, opt => opt.MapFrom(src => src.IdUsuarioPapel))
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.UsuarioInterno.Cpf.CnpjCpfFormat()))
            ;
        }
    }
}