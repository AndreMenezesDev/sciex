using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class AuditoriaAplicacaoProfile : Profile
	{
		public AuditoriaAplicacaoProfile()
		{
			CreateMap<AuditoriaAplicacaoEntity, AuditoriaAplicacaoVM>()
			.ForMember(dest => dest.CodigoAplicacao, opt => opt.MapFrom(src => src.CodigoAplicacao))
			.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
			.ForMember(dest => dest.IdAuditoriaAplicacao, opt => opt.MapFrom(src => src.IdAuditoriaAplicacao))
			.ForMember(dest => dest.ListaAuditoria, opt => opt.MapFrom(src => src.ListaAuditoria.Where(q => q.AuditoriaAplicacao.IdAuditoriaAplicacao == q.IdAuditoriaAplicacao)));
			
		}
	}
}