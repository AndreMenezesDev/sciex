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
    public class DivisaoAtividadeProfile : Profile
    {
        public DivisaoAtividadeProfile()
        {
            CreateMap<CADSUF_DIVISAO_ATIVIDADE, DivisaoAtividadeDto>()
                .ForMember(dest => dest.IdDivisaoAtividade, opt => opt.MapFrom(src => src.DIV_ID))
                .ForMember(dest => dest.IdSecaoAtividade, opt => opt.MapFrom(src => src.SEC_ID))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.DIV_CO))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DIV_DS))

                 .ReverseMap()
                .ForMember(dest => dest.DIV_ID, opt => opt.MapFrom(src => src.IdDivisaoAtividade))
                .ForMember(dest => dest.SEC_ID, opt => opt.MapFrom(src => src.IdSecaoAtividade))
                .ForMember(dest => dest.DIV_CO, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.DIV_DS, opt => opt.MapFrom(src => src.Descricao));               
               }
    }
}
