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
    public class SecaoAtividadeProfile : Profile
    {
        public SecaoAtividadeProfile()
        {
            CreateMap<CADSUF_SECAO_ATIVIDADE, SecaoAtividadeDto>()
                .ForMember(dest => dest.IdSecaoAtividade, opt => opt.MapFrom(src => src.SEC_ID))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.SEC_CO))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.SEC_DS))

                 .ReverseMap()
                .ForMember(dest => dest.SEC_ID, opt => opt.MapFrom(src => src.IdSecaoAtividade))
                .ForMember(dest => dest.SEC_CO, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.SEC_DS, opt => opt.MapFrom(src => src.Descricao));               
               }
    }
}
