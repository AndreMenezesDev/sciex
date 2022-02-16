using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class DiligenciaAtividadeSetorVMProfile : Profile
    {
        public DiligenciaAtividadeSetorVMProfile()
        {
            CreateMap<DiligenciaAtividadeSetorVM, DiligenciaAtividadesSetorEntity>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CodigoSetor.ExtractNumbers()))
                .ForMember(dest => dest.Setor, opt => opt.MapFrom(src => string.Join("", src.DescricaoSetor)));
        }
    }
}