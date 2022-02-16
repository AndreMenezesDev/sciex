using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class ErroProcessamentoEntityProfile : Profile
	{
		public ErroProcessamentoEntityProfile()
		{
			CreateMap<ErroProcessamentoEntity, ErroProcessamentoVM>()
				.ForMember(dest => dest.IdPliMercadoria, opt => opt.MapFrom(src => src.Pli.PLIMercadoria.FirstOrDefault().IdPliMercadoria))
				.ForMember(dest => dest.LocalErro, opt => opt.MapFrom(src => src.CodigoNivelErro == (byte)EnumErroProcessamentoNivelErro.PLI ? EnumErroProcessamentoNivelErro.PLI.ToString().Replace("_", " ") :
				src.CodigoNivelErro == (byte)EnumErroProcessamentoNivelErro.MERCADORIA ? EnumErroProcessamentoNivelErro.MERCADORIA.ToString().Replace("_", " ") :
				src.CodigoNivelErro == (byte)EnumErroProcessamentoNivelErro.ITEM ? EnumErroProcessamentoNivelErro.ITEM.ToString().Replace("_", " ") : "ERROR"))
				.ForMember(dest => dest.MensagemErro, opt => opt.MapFrom(src => src.ErroMensagem.Descricao == "" ? "" : src.ErroMensagem.Descricao))
				.ForMember(dest => dest.OrigemErro, opt => opt.MapFrom(src => src.ErroMensagem.ToString() == "" ? "" : src.ErroMensagem.CodigoSistemaOrigem == 5 ? "ANÁLISE VISUAL" : ((EnumSistemaOrigem)src.ErroMensagem.CodigoSistemaOrigem).ToString()));
		}
	}
}
