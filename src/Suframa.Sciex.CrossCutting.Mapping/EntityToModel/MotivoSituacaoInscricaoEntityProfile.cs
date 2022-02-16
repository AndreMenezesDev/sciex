using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class MotivoSituacaoInscricaoEntityProfile : Profile
	{
		public MotivoSituacaoInscricaoEntityProfile()
		{
			CreateMap<MotivoSituacaoInscricaoEntity, MotivoSituacaoInscricaoVM>();

			CreateMap<MotivoSituacaoInscricaoEntity, PendenciaCadastralVM>();
		}
	}
}