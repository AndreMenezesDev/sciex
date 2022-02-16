using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class AliArquivoEnvioEntityProfile : Profile
	{
		public AliArquivoEnvioEntityProfile()
		{
			CreateMap<AliArquivoEnvioEntity, AliArquivoEnvioVM>()
				.ForMember(dest => dest.DescricaoStatusEnvioSiscomex, opt => opt.MapFrom(src => ((EnumAliAquivoStatus)src.CodigoStatusEnvioSiscomex).ToString().Replace("_", " ")));
		}
	}
}
