using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
	public class ProtocoloProcedureProfile : Profile
	{
		public ProtocoloProcedureProfile()
		{
			CreateMap<ProtocoloProcedure, ProtocoloVM>()
				.ForMember(dest => dest.IdProtocolo, opt => opt.MapFrom(src => src.PRT_ID))
				.ForMember(dest => dest.NumeroSequencial, opt => opt.MapFrom(src => src.PRT_NU_SEQ))
				.ForMember(dest => dest.Ano, opt => opt.MapFrom(src => src.PRT_NU_ANO))
				.ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.DATA_PROTOCOLO))
				.ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.CPF_CNPJ))
				.ForMember(dest => dest.NomeRazaoSocial, opt => opt.MapFrom(src => src.NOME_RAZAOSOCIAL))
				.ForMember(dest => dest.DescricaoServico, opt => opt.MapFrom(src => src.SRV_DS))
				.ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.SPR_DS))
				.ForMember(dest => dest.DataDesignacao, opt => opt.MapFrom(src => src.DATA_DESIGNACAO))
				.ForMember(dest => dest.DiasRestantes, opt => opt.MapFrom(src => src.DIAS_RESTANTES))
				.ForMember(dest => dest.IdStatusProtocolo, opt => opt.MapFrom(src => src.SPR_ID))
				.ForMember(dest => dest.NomeUsuarioInterno, opt => opt.MapFrom(src => src.USI_NO));
		}
	}
}