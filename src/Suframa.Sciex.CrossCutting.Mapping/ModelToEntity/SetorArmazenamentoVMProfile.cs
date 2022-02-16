using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
	public class SetorArmazenamentoVMProfile : Profile
	{
		public SetorArmazenamentoVMProfile()
		{
			CreateMap<SetorArmazenamentoVM, SetorArmazenamentoEntity>();
		}
	}
}
