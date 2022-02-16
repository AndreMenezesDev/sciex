using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class PEProdutoPaisEntityProfile : Profile
	{
		public PEProdutoPaisEntityProfile()
		{
			CreateMap<PEProdutoPaisEntity, PEProdutoPaisVM>()
				.ForMember(dest => dest.ValorDolarFormatado, opt => opt.MapFrom(src => src.ValorDolar.ToString("N")))
				.ForMember(dest => dest.QuantidadeFormatado, opt => opt.MapFrom(src => src.Quantidade.ToString("N5")));

		}
	}
}
