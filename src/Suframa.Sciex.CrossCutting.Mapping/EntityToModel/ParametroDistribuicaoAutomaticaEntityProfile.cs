using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class ParametroDistribuicaoAutomaticaEntityProfile : Profile
    {
        public ParametroDistribuicaoAutomaticaEntityProfile()
        {
            CreateMap<ParametroDistribuicaoAutomaticaEntity, ParametroDistribuicaoAutomaticaVM>()
                .ForMember(dest => dest.DescricaoUnidadeCadastradora, opt => opt.MapFrom(src => src.UnidadeCadastradora.Descricao));
        }
    }
}