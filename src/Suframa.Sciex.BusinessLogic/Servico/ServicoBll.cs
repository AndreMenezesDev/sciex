using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic
{
	public class ServicoBll : IServicoBll
	{
		private readonly IUnitOfWork _uow;

		public ServicoBll(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public IEnumerable<ServicoVM> Listar(ServicoVM servicoVM)
		{
			if (servicoVM == null)
			{
				return _uow.QueryStack.Servico.Listar<ServicoVM>().OrderByDescending(o => o.Descricao);
			}

			var servicos = _uow.QueryStack.Servico
				.Listar(x =>
					(!servicoVM.TipoRequerimentoGrupo.HasValue ||
						(
							servicoVM.TipoRequerimentoGrupo.Value == EnumTipoRequerimentoGrupo.Externo &&
							x.TipoRequerimento.Any(y => TipoRequerimentoGrupoDto.Externo.Contains(y.IdTipoRequerimento))
						 )
					) &&
					(!servicoVM.IdServico.HasValue || servicoVM.IdServico.Value == x.IdServico)
				)
				.OrderByDescending(o => o.Descricao);

			if (servicos == null || !servicos.Any()) { return null; }

			var lista = new List<ServicoVM>();

			foreach (var servico in servicos)
			{
				if (servico.TipoRequerimento == null) { continue; }

				var _servicoVM = Mapper.Map<ServicoVM>(servico);

				var servicoTiposRequerimento = servico.TipoRequerimento.Select(x => x.IdTipoRequerimento);

				if (servicoTiposRequerimento.Any(x => TipoRequerimentoGrupoDto.ExternoExistente.Contains(x)))
				{
					_servicoVM.TipoRequerimentoGrupo = EnumTipoRequerimentoGrupo.ExternoExistente;
				}
				else if (servicoTiposRequerimento.Any(x => TipoRequerimentoGrupoDto.Externo.Contains(x)))
				{
					_servicoVM.TipoRequerimentoGrupo = EnumTipoRequerimentoGrupo.Externo;
				}

				lista.Add(_servicoVM);
			}

			return lista;
		}

		public void Salvar(IEnumerable<ServicoVM> servicos)
		{
			foreach (var servico in servicos)
			{
				var servicoEntity = _uow.QueryStack.Servico.Selecionar(x => x.IdServico == servico.IdServico);
				servicoEntity.QuantidadeDiasAnalise = servico.QuantidadeDiasAnalise;
				_uow.CommandStack.Servico.Salvar(servicoEntity);
			}

			_uow.CommandStack.Save();
		}
	}
}