using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class NcmExcecaoEntityProfile : Profile
	{
		public NcmExcecaoEntityProfile()
		{
			CreateMap<NcmExcecaoEntity, NcmExcecaoVM>()
				.ForMember(dest => dest.DataInicioVigenciaFormatado, opt => opt.MapFrom(src => src.DataInicioVigencia.ToShortDateString()))
				.ForMember(dest => dest.CodigoNCM, opt => opt.MapFrom(src => src.Codigo.CodigoNcmOcidentalFormat()));				;
		}
	}
}