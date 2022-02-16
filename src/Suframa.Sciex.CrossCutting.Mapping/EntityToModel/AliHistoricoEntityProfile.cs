using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class AliHistoricoEntityProfile : Profile
	{
		public AliHistoricoEntityProfile()
		{
			CreateMap<AliHistoricoEntity, AliHistoricoVM>()
				.ForMember(dest => dest.CPFCNPJResponsavel, opt => opt.MapFrom(src => src.CPFCNPJResponsavel.CnpjCpfFormat() ))
				.ForMember(dest => dest.LoginResponsavel, opt => opt.MapFrom(src => src.PliMercadoria.Pli.Cnpj.CnpjFormat()))
				.ForMember(dest => dest.DataFormadata, opt => opt.MapFrom(src => src.DataOperacao.ToShortDateString()+ " "+src.DataOperacao.Hour.ToString("D2")+":"+src.DataOperacao.Minute.ToString("D2")))
				.ForMember(dest => dest.DescricaoStatus, opt => opt.MapFrom(src => retornoDescricaoStatus(src.StatusAliAnterior, (src.StatusLiAnterior.HasValue ? src.StatusLiAnterior.Value : 0))))
				;
		}

		public string retornoDescricaoStatus(long statusAli, long statusLi)
		{
			if (statusAli == 3 && statusLi == 1)
			{
				return "LI SOLICITADA PARA CANCELAMENTO PELO IMPORTADOR";
			}

			if (statusAli == 6 && statusLi == 1)
			{
				return "LI ENVIADA AO SISCOMEX PARA CANCELAMENTO";
			}

			if (statusAli == 6 && statusLi == 3)
			{
				return "RESPOSTA DO SISCOMEX AO CANCELAMENTO DE LI";
			}
			else
			{
				return "";
			}
		}
	}
}
