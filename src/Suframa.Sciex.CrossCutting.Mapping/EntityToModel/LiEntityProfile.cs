using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class LiEntityProfile : Profile
	{
		public LiEntityProfile()
		{
			CreateMap<LiEntity, LiVM>()
				.ForMember(dest => dest.DescricaoStatus, opt => opt.MapFrom(src => ((EnumLiStatus)src.Status).ToString().Replace("_", " ")))
				.ForMember(dest => dest.DescricaoStatusAcao, opt => opt.MapFrom(src => retornoDescricaoStatus(src.PliMercadoria.Ali.Status, (src.PliMercadoria.Li.Status))))
				.ForMember(dest => dest.StatusAli, opt => opt.MapFrom(src => src.PliMercadoria.Ali.Status))
				.ForMember(dest => dest.TipoLi, opt => opt.MapFrom(src => src.TipoLi))
				.ForMember(dest => dest.NumeroALI, opt => opt.MapFrom(src => src.PliMercadoria.Ali.NumeroAli))
				.ForMember(dest => dest.CNPJEmpresa, opt => opt.MapFrom(src => src.PliMercadoria.Pli.Cnpj.CnpjFormat()))
				.ForMember(dest => dest.RazaoSocialEmpresa, opt => opt.MapFrom(src => src.PliMercadoria.Pli.RazaoSocial))
				.ForMember(dest => dest.NumeroPLIFormatado, opt => opt.MapFrom(src => src.PliMercadoria.Pli.Ano.ToString() + "/" + src.PliMercadoria.Pli.NumeroPli.ToString("d6")))
				.ForMember(dest => dest.NumeroNCM, opt => opt.MapFrom(src => src.PliMercadoria.CodigoNCMMercadoria))
				.ForMember(dest => dest.DataCadastroFormatado, opt => opt.MapFrom(src => src.DataCadastro.ToShortDateString()))
				.ForMember(dest => dest.DescricaoNCMMercadoria, opt => opt.MapFrom(src => src.PliMercadoria.DescricaoNCMMercadoria))
				.ForMember(dest => dest.NumeroLiMascarado, opt => opt.MapFrom(src => src.NumeroLi == null ? "" : src.NumeroLi.Value.ToString("D10")))
				.ForMember(dest => dest.NumeroReferencia, opt=> opt.MapFrom(src => src.PliMercadoria.Pli.NumeroLIReferencia))
			;
		}

		public string retornoDescricaoStatus(int statusAli, long statusLi)
		{
			if (statusAli == 3 && statusLi == 1)
			{
				return "LI DEFERIDA";
			}

			if (statusAli == 6 && statusLi == 1)
			{
				return "LI SOLICITADA PARA CANCELAMENTO";
			}

			if (statusAli == 6 && statusLi == 3)
			{
				return "LI ENVIADA AO SISCOMEX PARA CANCELAMENTO";
			}

			if (statusAli == 7 && statusLi == 4)
			{
				return "LI CANCELADA PELO IMPORTADOR";
			}

			return "";
		}
	}
}
