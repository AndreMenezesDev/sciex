using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class NcmProfile : Profile
	{
		public NcmProfile()
		{
			CreateMap<NcmEntity, NcmVM>()
			.ForMember(dest => dest.CodigoNCM, opt => opt.MapFrom(src => src.CodigoNCM.CodigoNcmOcidentalFormat()))
			.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao.ToUpper()));
		}
	}
}