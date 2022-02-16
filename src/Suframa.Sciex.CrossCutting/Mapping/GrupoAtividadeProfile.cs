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
    public class GrupoAtividadeProfile : Profile
    {
        public GrupoAtividadeProfile()
        {
            CreateMap<CADSUF_GRUPO_ATIVIDADE, GrupoAtividadeDto>()
                .ForMember(dest => dest.IdGrupoAtividade, opt => opt.MapFrom(src => src.GP_ID))
                .ForMember(dest => dest.IdDivisaoAtividade, opt => opt.MapFrom(src => src.DIV_ID))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.GP_CO))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.GP_DS))

                 .ReverseMap()
                .ForMember(dest => dest.GP_ID, opt => opt.MapFrom(src => src.IdGrupoAtividade))
                .ForMember(dest => dest.DIV_ID, opt => opt.MapFrom(src => src.IdDivisaoAtividade))
                .ForMember(dest => dest.GP_CO, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.GP_DS, opt => opt.MapFrom(src => src.Descricao));
               }
    }
}
