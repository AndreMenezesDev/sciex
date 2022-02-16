using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
	public class VWPessoaJuridicaSocioAprovadoEntityProfile : Profile
	{
		public VWPessoaJuridicaSocioAprovadoEntityProfile()
		{
			CreateMap<VWPessoaJuridicaSocioAprovadoEntity, QuadroSocietarioVM>()
				.ForMember(dest => dest.TipoPessoa, opt => opt.MapFrom(src => (EnumTipoPessoa)src.TipoPessoa))
				.ForMember(dest => dest.DescricaoQualificacao, opt => opt.MapFrom(src => src.Qualificacao.Descricao))
				.ForMember(dest => dest.CnpjCpf, opt => opt.MapFrom(src => src.CnpjCpf.CnpjCpfUnformat()))
				.ForMember(dest => dest.DescricaoPais, opt => opt.MapFrom(src => src.Pais.Descricao))
				.ForMember(dest => dest.DescricaoNaturezaJuridica, opt => opt.MapFrom(src => src.NaturezaJuridica.Descricao))
				.ForMember(dest => dest.TipoSocio, opt => opt.MapFrom(src => (EnumTipoSocio)src.TipoSocio));
		}
	}
}