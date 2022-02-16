using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class DiligenciaAtividadeVMProfile : Profile
    {
        public DiligenciaAtividadeVMProfile()
        {
            CreateMap<DiligenciaAtividadeVM, DiligenciaAtividadesEntity>()
                .ForMember(dest => dest.CodigoSubclasse, opt => opt.MapFrom(src => src.CodigoSubclasse.ExtractNumbers()))
                .ForMember(dest => dest.DescricaoSubclasse, opt => opt.MapFrom(src => src.DescricaoSubclasse))
                .ForMember(dest => dest.StatusAtividadeExercida, opt => opt.MapFrom(src => src.IsAtividadeExercida))
                .ForMember(dest => dest.DiligenciaAtividadeSetor, opt => opt.MapFrom(src => src.Setor));
        }
    }
}