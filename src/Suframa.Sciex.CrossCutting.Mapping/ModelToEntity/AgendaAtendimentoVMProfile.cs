using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class AgendaAtendimentoVMProfile : Profile
    {
        public AgendaAtendimentoVMProfile()
        {
            CreateMap<AgendaAtendimentoVM, AgendaAtendimentoEntity>()
                .ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.CpfCnpj.CnpjCpfUnformat()))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => (int)src.Tipo))
                .ForMember(dest => dest.DataAgendamento, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.DataAlteracao, opt => opt.MapFrom(src => DateTime.Now))
            ;
        }
    }
}