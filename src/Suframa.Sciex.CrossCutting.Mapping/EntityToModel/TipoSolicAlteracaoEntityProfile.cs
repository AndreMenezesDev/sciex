using AutoMapper;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.Mapping.EntityToModel
{
	public class TipoSolicAlteracaoEntityProfile : Profile
	{
			public TipoSolicAlteracaoEntityProfile()
			{
				CreateMap<TipoSolicAlteracaoEntity, TipoSolicAlteracaoVM>();
			}
		
	}
}
