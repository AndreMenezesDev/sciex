using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
	public class ProtocoloVMProfile : Profile
	{
		public ProtocoloVMProfile()
		{
			CreateMap<ProtocoloVM, ProtocoloEntity>()
			   .ForMember(dest => dest.IdStatusProtocolo, opt => opt.MapFrom(src => (int)src.IdStatusProtocolo))
			   .ReverseMap();

			CreateMap<ProtocoloVM, WorkflowProtocoloEntity>()
				.ForMember(dest => dest.Data, opt => opt.MapFrom(src => DateTime.Now));

			CreateMap<ProtocoloVM, ProtocoloProcedure>()
				.ForMember(dest => dest.CPF_CNPJ, opt => opt.MapFrom(src => src.CpfCnpj))
				.ForMember(dest => dest.NOME_RAZAOSOCIAL, opt => opt.MapFrom(src => src.NomeRazaoSocial))
				.ForMember(dest => dest.DATA_DESIGNACAO_INICIAL, opt => opt.MapFrom(src => src.DataDesignacaoInicial))
				.ForMember(dest => dest.DATA_DESIGNACAO_FINAL, opt => opt.MapFrom(src => src.DataDesignacaoFinal))
				.ForMember(dest => dest.DATA_PROTOCOLO_INICIAL, opt => opt.MapFrom(src => src.DataInclusaoInicial))
				.ForMember(dest => dest.DATA_PROTOCOLO_FINAL, opt => opt.MapFrom(src => src.DataInclusaoFinal))
				.ForMember(dest => dest.ANO_PROTOCOLO, opt => opt.MapFrom(src => src.Ano))
				.ForMember(dest => dest.NUMERO_PROTOCOLO, opt => opt.MapFrom(src => src.NumeroSequencial))
				.ForMember(dest => dest.ID_UNIDADE, opt => opt.MapFrom(src => src.IdUnidadeCadastradora))
				.ForMember(dest => dest.IS_GRUPOPROTOCOLOANALISE, opt => opt.MapFrom(src => (int)src.StatusProtocoloGrupo))
				.ForMember(dest => dest.ID_SITUACAO_PROTOCOLO, opt => opt.MapFrom(src => (int?)src.IdStatusProtocolo))
				.ForMember(dest => dest.ID_TIPO_SERVICO, opt => opt.MapFrom(src => src.IdServico))
				.ForMember(dest => dest.ID_RESPONSAVEL, opt => opt.MapFrom(src => src.IdUsuarioInterno))
				.ForMember(dest => dest.SORT, opt => opt.MapFrom(src => src.Sort))
				.ForMember(dest => dest.REVERSE, opt => opt.MapFrom(src => src.Reverse))
				.ForMember(dest => dest.PAGE, opt => opt.MapFrom(src => src.Page))
				.ForMember(dest => dest.SIZE, opt => opt.MapFrom(src => src.Size))
				.ForMember(dest => dest.USI_NO, opt => opt.MapFrom(src => src.NomeUsuarioInterno))
				.ForMember(dest => dest.TOTAL, opt => opt.MapFrom(src => src.Total));
		}
	}
}