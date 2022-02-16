using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Security.Cryptography;

namespace Suframa.Sciex.CrossCutting.Mapping.EntityToModel
{
	public class DIEntradaEntityProfile: Profile
	{
		public DIEntradaEntityProfile()
		{
			CreateMap<DiEntradaEntity, DIEntradaVM>()
				.ForMember(dest => dest.Identificador, opt => opt.MapFrom( src =>
							src.DiArquivoEntrada.Id))
				.ForMember(dest => dest.DataValidacao, opt => opt.MapFrom(src => 
							src.DiArquivoEntrada.DataHoraInicioProcesso.Value.ToString("dd/MM/yyyy hh:mm")))
				.ForMember(dest => dest.StatusValidacao, opt => opt.MapFrom(src =>
							src.Situacao == 2 ? "SUCESSO" : "FALHA"))
				.ForMember(dest => dest.QtdErros, opt => opt.MapFrom(src => src.ErroProcessamento.Count));
		}
	}
}
