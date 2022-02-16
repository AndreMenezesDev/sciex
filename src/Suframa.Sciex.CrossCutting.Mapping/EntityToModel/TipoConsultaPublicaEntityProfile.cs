using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class TipoConsultaPublicaEntityProfile : Profile
    {
        public TipoConsultaPublicaEntityProfile()
        {
            CreateMap<TipoConsultaPublicaEntity, ConsultaPublicaVM>()
                .ForMember(dest => dest.DataRestricao, opt => opt.MapFrom(src => src.ConsultaPublica.FirstOrDefault().DataRestricao));
        }
    }
}