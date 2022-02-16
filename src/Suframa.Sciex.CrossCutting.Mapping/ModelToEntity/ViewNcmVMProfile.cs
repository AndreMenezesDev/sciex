using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
	public class ViewNcmVMProfile : Profile
	{
		public ViewNcmVMProfile()
		{
			CreateMap<ViewNcmVM, ViewNcmEntity>();
		}
	}
}