using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.SuperStructs;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
	public class VWQuadroPessoalAprovadoEntityProfile : Profile
	{
		public VWQuadroPessoalAprovadoEntityProfile()
		{
			CreateMap<VWQuadroPessoalAprovadoEntity, QuadroPessoalVM>()
				.ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => Cpf.Unmask(src.Cpf)))
				.ForMember(dest => dest.NomeArquivo, opt => opt.MapFrom(src => src.Arquivo.Nome));
		}
	}
}