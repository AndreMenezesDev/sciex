using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.EntityToModel
{
	public class EstruturaPropriaPliArquivoEntityProfile : Profile
	{
		public EstruturaPropriaPliArquivoEntityProfile()
		{
			CreateMap<EstruturaPropriaPliArquivoEntity, EstruturaPropriaPLIVM>();
		}
	}
}
