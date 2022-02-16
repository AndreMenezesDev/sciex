using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class TipoDeclaracaoEntityProfile : Profile
	{
		public TipoDeclaracaoEntityProfile()
		{
			CreateMap<TipoDeclaracaoEntity, TipoDeclaracaoVM>()
				.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao.ToUpper()));
		}
	}
}