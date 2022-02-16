﻿using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class LiArquivoEntityProfile : Profile
	{
		public LiArquivoEntityProfile()
		{
			CreateMap<LiArquivoEntity, LiArquivoVM>();
		}
	}
}
