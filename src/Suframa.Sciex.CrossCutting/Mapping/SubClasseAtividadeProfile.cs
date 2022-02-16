using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class SubClasseAtividadeProfile : Profile
    {
        public SubClasseAtividadeProfile()
        {
            CreateMap<CADSUF_SUBCLASSE_ATIVIDADE, SubClasseAtividadeDto>()
                .ForMember(dest => dest.IdSubClasseAtividade, opt => opt.MapFrom(src => src.SBC_ID))
                .ForMember(dest => dest.IdClasseAtividade, opt => opt.MapFrom(src => src.CLA_ID))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.SBC_CO))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.SBC_DS))
                .ForMember(dest => dest.DataAlteracao, opt => opt.MapFrom(src => src.SBC_DT_ALTERACAO))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.SBC_DT_INCLUSAO))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.SBC_ST))
                 .ReverseMap()
                .ForMember(dest => dest.SBC_ID, opt => opt.MapFrom(src => src.IdSubClasseAtividade))
                .ForMember(dest => dest.CLA_ID, opt => opt.MapFrom(src => src.IdClasseAtividade))
                .ForMember(dest => dest.SBC_CO, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.SBC_DS, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.SBC_DT_ALTERACAO, opt => opt.MapFrom(src => src.DataAlteracao))
                .ForMember(dest => dest.SBC_DT_INCLUSAO, opt => opt.MapFrom(src => src.DataInclusao))
                .ForMember(dest => dest.SBC_ST, opt => opt.MapFrom(src => src.Status));
        }
    }
}
