using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
    public class WorkflowProtocoloVMProfile : Profile
    {
        public WorkflowProtocoloVMProfile()
        {
            CreateMap<WorkflowProtocoloVM, WorkflowProtocoloEntity>()
             .ForMember(dest => dest.IdStatusProtocolo, opt => opt.MapFrom(src => (int)src.IdStatusProtocolo));
            //.ForMember(dest => dest.StatusProtocolo, opt => opt.ResolveUsing(model => new StatusProtocoloEntity { Descricao = model.DescricaoStatusProtocolo }));
        }
    }
}