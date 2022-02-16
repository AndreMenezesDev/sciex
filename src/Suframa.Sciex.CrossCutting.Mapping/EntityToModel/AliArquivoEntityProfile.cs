using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.Mapping.EntityToModel
{
	public class AliArquivoEntityProfile : Profile
	{
		public AliArquivoEntityProfile()
		{
			CreateMap<AliArquivoEntity, AliArquivoVM>();
		}
	}
}
