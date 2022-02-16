using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class SolicitacaoPliEntityProfile : Profile
	{
		public SolicitacaoPliEntityProfile()
		{
			CreateMap<SolicitacaoPliEntity, SolicitacaoPliVM>()
				.ForMember(dest => dest.DataValidacao, opt => opt.MapFrom(src => src.EstruturaPropriaPli.DataInicioProcessamento.Value.ToShortDateString()))
				.ForMember(dest => dest.StatusSolicitacaoNome, opt => opt.MapFrom(src =>
				src.StatusSolicitacao == 2 ? "SUCESSO" : "FALHA"))
				.ForMember(dest => dest.QtdErrosPli, opt => opt.MapFrom(src => src.ErroProcessamento.Count))
				.ForMember(dest => dest.QtdSucessoPli, opt => opt.MapFrom(src => src.EstruturaPropriaPli.QuantidadePLIProcessadoSucesso))
				.ForMember(dest => dest.CnpjEmpresa, opt => opt.MapFrom(src => src.CnpjEmpresa.CnpjCpfFormat()))
				.ForMember(dest => dest.NumeroPliSuframa, opt => opt.MapFrom(src => src.NumeroPLI.HasValue ? src.AnoPLI.Value.ToString() + "/"+ src.NumeroPLI.Value.ToString("D6") : "" ))
				.ForMember(dest => dest.DataInicioProcessamento, opt => opt.MapFrom(src => src.EstruturaPropriaPli.DataInicioProcessamento));
				
		}
	}
}
