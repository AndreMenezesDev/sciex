using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class AgendaAtendimentoEntityProfile : Profile
    {
        public AgendaAtendimentoEntityProfile()
        {
            CreateMap<AgendaAtendimentoEntity, AgendaAtendimentoVM>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => (EnumTipoAgendaAtendimento)src.Tipo))
                .ForMember(dest => dest.HorarioAtendimento, opt => opt.MapFrom(src => src.CalendarioHora != null ? src.CalendarioHora.FirstOrDefault().HorarioAtendimento : null))
                .ForMember(dest => dest.DataAtendimento, opt => opt.MapFrom(src => src.CalendarioHora != null ? src.CalendarioHora.FirstOrDefault().HorarioAtendimento : null))
                .ForMember(dest => dest.DescricaoServico, opt => opt.MapFrom(src => src.Servico == null ? null : src.Servico.Descricao))
                .ForMember(dest => dest.DescricaoUnidadeCadastradora, opt => opt.MapFrom(src => src.UnidadeCadastradora == null ? null : src.UnidadeCadastradora.Descricao))
                .ForMember(dest => dest.NumeroSenha, opt => opt.MapFrom(src => src.IdAgendaAtendimento.ToString().PadLeft(6, '0')))
                .ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.CpfCnpj.CnpjCpfFormat()))
                .ForMember(dest => dest.Documentos, opt => opt.MapFrom(src => (src.Servico == null || src.Servico.TipoRequerimento == null || src.Servico.TipoRequerimento.SelectMany(x => x.ListaDocumento) == null) ? null : src.Servico.TipoRequerimento.SelectMany(x => x.ListaDocumento).Select(x => x.TipoDocumento.Descricao)))
            ;
        }
    }
}