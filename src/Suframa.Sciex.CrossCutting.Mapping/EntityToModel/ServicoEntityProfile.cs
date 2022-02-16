using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class ServicoEntityProfile : Profile
    {
        public ServicoEntityProfile()
        {
            CreateMap<ServicoEntity, ServicoVM>()
                .ForMember(dest => dest.QuantidadeDiasAnalise, opt => opt.MapFrom(src => src.QuantidadeDiasAnalise.HasValue ? src.QuantidadeDiasAnalise : 0));
        }
    }
}