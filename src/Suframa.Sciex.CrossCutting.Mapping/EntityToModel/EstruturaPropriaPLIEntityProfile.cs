using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.EntityToModel
{
	public class EstruturaPropriaPliEntityProfile : Profile
	{
		public EstruturaPropriaPliEntityProfile()
		{
			CreateMap<EstruturaPropriaPliEntity, EstruturaPropriaPLIVM>()
				.ForMember(dest => dest.CNPJImportador, opt => opt.MapFrom(src => src.CNPJImportador.ToString().CnpjCpfFormat()))
				.ForMember(dest => dest.QuantidadePliConcatenado, opt => opt.MapFrom(src => src.QuantidadePLIArquivo + " - " + src.QuantidadePLIProcessadoSucesso + " - " + src.QuantidadePLIProcessadoFalha))
				.ForMember(dest => dest.StatusValidacaoArquivoConcatenado, opt => opt.MapFrom(src => 
					src.DescricaoPendenciaImportador != null ? "EMPRESA BLOQUEADA" : 
					src.StatusProcessamentoArquivo == (byte)EnumStatusProcessamentoPli.ENVIADO_A_SUFRAMA ? EnumStatusProcessamentoPli.ENVIADO_A_SUFRAMA.ToString().Replace("_", " ") :
					src.StatusProcessamentoArquivo == (byte)EnumStatusProcessamentoPli.EM_EXTRAÇÃO ? EnumStatusProcessamentoPli.EM_EXTRAÇÃO.ToString().Replace("_", " ") :
					src.StatusProcessamentoArquivo == (byte)EnumStatusProcessamentoPli.EM_VALIDAÇÃO ? EnumStatusProcessamentoPli.EM_VALIDAÇÃO.ToString().Replace("_", " ") :
					src.StatusProcessamentoArquivo == (byte)EnumStatusProcessamentoPli.VALIDADO ? EnumStatusProcessamentoPli.VALIDADO.ToString() : ""));
		}
	}
}
