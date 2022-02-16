using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class ManterAtividadeEconomicaVMProfile : Profile
    {
        public ManterAtividadeEconomicaVMProfile()
        {
            CreateMap<ManterAtividadeEconomicaVM, SubclasseAtividadeEntity>();

            CreateMap<ManterAtividadeEconomicaVM, SubClasseAtividadeDto>()
                .ForMember(dest => dest.CodigoGrupoAtividade, opt => opt.Ignore())
                .ForMember(dest => dest.CodigoDivisaoAtividade, opt => opt.Ignore())
                .ForMember(dest => dest.CodigoClasseAtividade, opt => opt.Ignore());
        }
    }
}