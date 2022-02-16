using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class PRCDetalheHistoricoInsumoEntity : Profile
	{
		public PRCDetalheHistoricoInsumoEntity()
		{
			CreateMap<PRCDetalheHistoricoInsumoEntity, PRCHistoricoDetalheInsumoVM>();
		}
	}
}