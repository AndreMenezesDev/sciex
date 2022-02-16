using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class TaxaPliEntityProfile : Profile
	{
		public TaxaPliEntityProfile()
		{
			CreateMap<TaxaPliEntity, TaxaPliVM>();
				//.ForMember(dest=> dest.TaxaPliDebito, opt=> opt.MapFrom(orig=> orig.TaxaPliDebito.FirstOrDefault(q=> q.IdPli == q.TaxaPli.IdPli)));			
		}
	}
}
