using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class DiligenciaAnexoEntityProfile : Profile
    {
        public DiligenciaAnexoEntityProfile()
        {
            CreateMap<DiligenciaAnexosEntity, DiligenciaAnexoVM>()
                .ForMember(dest => dest.NomeArquivo, opt => opt.MapFrom(src => src.Arquivo.Nome));
        }
    }
}