using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.DataAccess.RestService.ApiDto;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class VWPessoaJuridicaAdministradorAprovadoEntityProfile : Profile
	{
		public VWPessoaJuridicaAdministradorAprovadoEntityProfile()
		{
			CreateMap<VWPessoaJuridicaAdministradorAprovadoEntity, PessoaJuridicaAdministradorEntity>()
				.ForMember(dest => dest.ConsultaPublica, opt => opt.Ignore());
		}
	}
}