using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.SuperStructs;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class QuadroPessoalVMProfile : Profile
    {
        public QuadroPessoalVMProfile()
        {
            CreateMap<QuadroPessoalVM, QuadroPessoalEntity>()
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => Cpf.Unmask(src.Cpf)));
        }
    }
}