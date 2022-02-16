using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class PEProdutoEntityProfile : Profile
	{
		public PEProdutoEntityProfile()
		{
			CreateMap<PEProdutoEntity, PEProdutoVM>()
				.ForMember(dest => dest.ValorDolarFormatado, opt => opt.MapFrom(src => src.ValorDolar.ToString("N")))
				.ForMember(dest => dest.QtdFormatado, opt => opt.MapFrom(src => src.Qtd.ToString("N5")));
				
		}
	}
}
