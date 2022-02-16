using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class CampoSistemaEntityProfile : Profile
    {
        public CampoSistemaEntityProfile()
        {
            CreateMap<CampoSistemaEntity, CampoSistemaDto>();

            CreateMap<CampoSistemaEntity, CampoSistemaVM>();
        }
    }
}