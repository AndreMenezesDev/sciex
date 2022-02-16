using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.SuperStructs;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class DadosSolicitanteEntityProfile : Profile
    {
        public DadosSolicitanteEntityProfile()
        {
            CreateMap<DadosSolicitanteEntity, DadosSolicitanteVM>()
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => Cpf.Mask(src.Cpf)));
        }
    }
}