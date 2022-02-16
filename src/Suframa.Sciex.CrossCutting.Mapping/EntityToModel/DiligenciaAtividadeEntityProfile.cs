using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class DiligenciaAtividadeEntityProfile : Profile
    {
        public DiligenciaAtividadeEntityProfile()
        {
            CreateMap<DiligenciaAtividadesEntity, DiligenciaAtividadeVM>()
                .ForMember(dest => dest.CodigoSubclasse, opt => opt.MapFrom(src => src.CodigoSubclasse))
                .ForMember(dest => dest.DescricaoSubclasse, opt => opt.MapFrom(src => src.DescricaoSubclasse))
                .ForMember(dest => dest.IdDiligenciaAtividade, opt => opt.MapFrom(src => src.IdDiligenciaAtividade))
                .ForMember(dest => dest.IsAtividadeExercida, opt => opt.MapFrom(src => src.StatusAtividadeExercida))
                .ForMember(dest => dest.Setor, opt => opt.MapFrom(src => src.DiligenciaAtividadeSetor));
        }
    }
}