using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class DiligenciaAtividadeSetorEntityProfile : Profile
    {
        public DiligenciaAtividadeSetorEntityProfile()
        {
            CreateMap<DiligenciaAtividadesSetorEntity, DiligenciaAtividadeSetorVM>()
                .ForMember(dest => dest.CodigoSetor, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.DescricaoSetor, opt => opt.MapFrom(src => src.Setor));
        }
    }
}