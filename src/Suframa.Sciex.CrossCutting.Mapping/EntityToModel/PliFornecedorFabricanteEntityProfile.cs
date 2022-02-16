using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class PliFornecedorFabricanteEntityProfile : Profile
	{
		public PliFornecedorFabricanteEntityProfile()
		{
			CreateMap<PliFornecedorFabricanteEntity, PliFornecedorFabricanteVM>()
				.ForMember(dest => dest.CodigoDescricaoPaisFornecedorConcatenado, opt => opt.MapFrom(src => src.CodigoPaisFornecedor + " | " + src.DescricaoPaisFornecedor))
				.ForMember(dest => dest.CodigoDescricaoPaisFabricanteConcatenado, opt => opt.MapFrom(src => src.CodigoPaisFabricante + " | " + src.DescricaoPaisFabricante)
				
				
				);

		}
	}
}
