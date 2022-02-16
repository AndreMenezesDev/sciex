using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class PedidoCorrecaoEntityProfile : Profile
    {
        public PedidoCorrecaoEntityProfile()
        {
            CreateMap<PedidoCorrecaoEntity, PedidoCorrecaoVM>();

            CreateMap<PedidoCorrecaoEntity, ResumoVM>()
                .ForMember(dest => dest.Secao, opt => opt.MapFrom(src => src.CampoSistema.Secao))
                .ForMember(dest => dest.Campo, opt => opt.MapFrom(src => src.CampoSistema.Campo))
                .ForMember(dest => dest.CampoTela, opt => opt.MapFrom(src => src.CampoSistema.CampoTela))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.CampoSistema.DescricaoCampo))
                .ForMember(dest => dest.ValorDe, opt => opt.MapFrom(src => src.CampoDe))
                .ForMember(dest => dest.ValorPara, opt => opt.MapFrom(src => src.CampoPara))
                .ForMember(dest => dest.Acao, opt => opt.MapFrom(src => src.Acao))
                .ForMember(dest => dest.Justificativa, opt => opt.MapFrom(src => src.Justificativa))
                .ForMember(dest => dest.DataSolicitacao, opt => opt.MapFrom(src => src.DataSolicitacao));
        }
    }
}