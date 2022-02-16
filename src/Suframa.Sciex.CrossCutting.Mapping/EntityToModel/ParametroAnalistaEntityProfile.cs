using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
	public class ParametroAnalistaEntityProfile : Profile
	{
		public ParametroAnalistaEntityProfile()
		{
			CreateMap<ParametroAnalistaEntity, ParametroAnalistaVM>()
				.ForMember(dest => dest.NomeUsuarioInterno, opt => opt.MapFrom(src => src.UsuarioInterno.Nome))
				.ForMember(dest => dest.IdsServicos, opt => opt.MapFrom(src => src.ParametroAnalistaServico.Select(x => x.IdServico)))
				.ForMember(dest => dest.DescricaoServicos, opt => opt.MapFrom(src => src.ParametroAnalistaServico.Select(x => x.Servico.Descricao)))
				.ForMember(dest => dest.ParametroAnalistaServico, opt => opt.MapFrom(src => src.ParametroAnalistaServico));
		}
	}
}