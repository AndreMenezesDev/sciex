using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.SuperStructs;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class VWPessoaFisicaAprovadoEntityProfile : Profile
	{
		public VWPessoaFisicaAprovadoEntityProfile()
		{
			CreateMap<VWPessoaFisicaAprovadoEntity, IdentificacaoPessoaFisicaVM>()
				.ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => Cpf.Unmask(src.Cpf)))
				.ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Cep.Codigo.ToString().PadLeft(8, '0')))
				.ForMember(dest => dest.ModalidadeTransportador, opt => opt.MapFrom(src => (EnumTipoTransportador?)src.Credenciamento.Where(x => x.IdTipoUsuario == (int)EnumTipoUsuario.PessoaFisicaTransportador).FirstOrDefault().ModalidadeTransportador));
		}
	}
}