using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.SuperStructs;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class PessoaJuridicaAdministradorEntityProfile : Profile
	{
		public PessoaJuridicaAdministradorEntityProfile()
		{
			CreateMap<PessoaJuridicaAdministradorEntity, QuadroAdministradoresVM>()
				.ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => Cpf.Unmask(src.Cpf)))
				.ForMember(dest => dest.NomeSocial, opt => opt.MapFrom(src => src.NomeSocial))
				.ForMember(dest => dest.DescricaoQualificacao, opt => opt.MapFrom(src => src.Qualificacao.Descricao));

			CreateMap<PessoaJuridicaAdministradorEntity, ConsultaPublicaResponsaveisVM>()
				.ForMember(dest => dest.IdPessoaJuridicaAdministrador, opt => opt.MapFrom(src => src.IdAdministrador))
				.ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => Cpf.Mask(src.Cpf)))
				.ForMember(dest => dest.NomeRazaoSocial, opt => opt.MapFrom(src => src.Nome));
		}
	}
}