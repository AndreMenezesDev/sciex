using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class FundamentoLegalEntityProfile : Profile
	{
		public FundamentoLegalEntityProfile()
		{
			CreateMap<FundamentoLegalEntity, FundamentoLegalVM>()
				.ForMember(dest => dest.IdFundamentoLegal, opt => opt.MapFrom(src => src.IdFundamentoLegal))
				.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));
		}
	}
}