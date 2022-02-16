using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class DIArquivoEntradaEntityProfile : Profile
	{
		public DIArquivoEntradaEntityProfile()
		{
			CreateMap<DiArquivoEntradaEntity, DIArquivoEntradaVM>().
				ForMember(dest => dest.quantidadeDiConcatenado, opt => opt.MapFrom( src => src.QuantidadeDi.ToString() + " " +
							src.QuantidadeDiProcessada.ToString() + " " + src.QuantidadeDiErro.ToString())).
				ForMember(dest => dest.DescricaoSituacaoLeitura, opt => opt.MapFrom( src => 
						  src.SituacaoLeitura == (byte) EnumSituacaoLeitura.LEITURA_NAO_EFETIVADA ?
							EnumSituacaoLeitura.LEITURA_NAO_EFETIVADA.ToString().Replace("_", " ") :
							src.SituacaoLeitura == (byte)EnumSituacaoLeitura.LEITURA_EFETIVADA ?
							EnumSituacaoLeitura.LEITURA_EFETIVADA.ToString().Replace("_", " ") :
							src.SituacaoLeitura == (byte)EnumSituacaoLeitura.ARQUIVO_PROCESSADO ?
							EnumSituacaoLeitura.ARQUIVO_PROCESSADO.ToString().Replace("_", " "):"")).
				ForMember(dest => dest.DataRecepcaoFormatada, opt => opt.MapFrom ( src => 
								src.DataHoraRecepcao.ToString().Replace("T", " " ))).
				ForMember(dest => dest.DataHoraInicioProcesso, opt => opt.MapFrom ( src =>
								src.DataHoraInicioProcesso.ToString().Replace("T", " ")));	
		}
	}
}
