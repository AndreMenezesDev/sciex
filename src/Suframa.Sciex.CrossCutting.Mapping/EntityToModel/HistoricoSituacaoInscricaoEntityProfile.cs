using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class HistoricoSituacaoInscricaoEntityProfile : Profile
    {
        public HistoricoSituacaoInscricaoEntityProfile()
        {
            CreateMap<HistoricoSituacaoInscricaoEntity, HistoricoSituacaoInscricaoVM>()
                .ForMember(dest => dest.DescricaoMotivoSituacaoInscricao, opt => opt.MapFrom(src => src.MotivoSituacaoInscricao.Descricao))
                .ForMember(dest => dest.DescricaoSituacaoInscricao, opt => opt.MapFrom(src => src.WorkflowSituacaoInscricao.SituacaoInscricao.Descricao))
                .ForMember(dest => dest.IdInscricaoCadastral, opt => opt.MapFrom(src => src.WorkflowSituacaoInscricao.IdInscricaoCadastral))
                .ForMember(dest => dest.DescricaoExplicacao, opt => opt.MapFrom(src => src.MotivoSituacaoInscricao.Explicacao))
                .ForMember(dest => dest.DescricaoOrientacao, opt => opt.MapFrom(src => src.MotivoSituacaoInscricao.Orientacao))
            ;
        }
    }
}