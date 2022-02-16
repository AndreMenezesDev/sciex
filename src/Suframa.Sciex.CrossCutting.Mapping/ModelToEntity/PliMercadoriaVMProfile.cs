using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class PliMercadoriaVMProfile : Profile
	{
		public PliMercadoriaVMProfile()
		{
			CreateMap<PliMercadoriaVM, PliMercadoriaEntity>()
				.ForMember(dest => dest.NumeroLiRetificador, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.NumeroLIRetificador) ? 0 : Convert.ToInt32(src.NumeroLIRetificador)));
		}
	}
}
