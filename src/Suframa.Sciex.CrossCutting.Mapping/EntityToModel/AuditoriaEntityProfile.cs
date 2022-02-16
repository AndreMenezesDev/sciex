using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class AuditoriaProfile : Profile
	{
		public AuditoriaProfile()
		{
			CreateMap<AuditoriaEntity, AuditoriaVM>()
			.ForMember(dest => dest.CpfCnpjResponsavel, opt => opt.MapFrom(src => src.CpfCnpjResponsavel))
			.ForMember(dest => dest.DataHoraAcao, opt => opt.MapFrom(src => src.DataHoraAcao))
			.ForMember(dest => dest.DescricaoAcao, opt => opt.MapFrom(src => src.DescricaoAcao))
			.ForMember(dest => dest.IdAuditoria, opt => opt.MapFrom(src => src.IdAuditoria))
			.ForMember(dest => dest.IdAuditoriaAplicacao, opt => opt.MapFrom(src => src.AuditoriaAplicacao.IdAuditoriaAplicacao))
			.ForMember(dest => dest.IdReferencia, opt => opt.MapFrom(src => src.IdReferencia))
			.ForMember(dest => dest.Justificativa, opt => opt.MapFrom(src => src.Justificativa))
			.ForMember(dest => dest.NomeResponsavel, opt => opt.MapFrom(src => src.NomeResponsavel))
			.ForMember(dest => dest.TipoAcao, opt => opt.MapFrom(src => src.TipoAcao));
		}
	}
}