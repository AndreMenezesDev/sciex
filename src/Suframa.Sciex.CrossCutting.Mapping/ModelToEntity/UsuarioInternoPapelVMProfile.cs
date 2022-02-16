using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class UsuarioInternoPapelVMProfile : Profile
    {
        public UsuarioInternoPapelVMProfile()
        {
            CreateMap<UsuarioInternoPapelVM, UsuarioPapelEntity>();
        }
    }
}