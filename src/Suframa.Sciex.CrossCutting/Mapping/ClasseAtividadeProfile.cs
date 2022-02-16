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
    public class ClasseAtividadeProfile : Profile
    {
        public ClasseAtividadeProfile()
        {
            CreateMap<CADSUF_CLASSE_ATIVIDADE, ClasseAtividadeDto>()
                .ForMember(dest => dest.IdClasseAtividade, opt => opt.MapFrom(src => src.CLA_ID))
                .ForMember(dest => dest.IdGrupoAtividade, opt => opt.MapFrom(src => src.GP_ID))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CLA_CO))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.CLA_DS))
               
                 .ReverseMap()
                .ForMember(dest => dest.CLA_ID, opt => opt.MapFrom(src => src.IdClasseAtividade))
                .ForMember(dest => dest.GP_ID, opt => opt.MapFrom(src => src.IdGrupoAtividade))
                .ForMember(dest => dest.CLA_CO, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.CLA_DS, opt => opt.MapFrom(src => src.Descricao));
               }
    }
}
