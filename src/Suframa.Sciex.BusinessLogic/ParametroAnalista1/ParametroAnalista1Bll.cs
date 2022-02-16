using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Resources;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic
{
	public class ParametroAnalista1Bll : IParametroAnalista1Bll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public ParametroAnalista1Bll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}

		public IEnumerable<ParametroAnalista1VM> Listar(ParametroAnalista1VM parametroAnalista1VM)
		{
			VerificarAnalistasNaoSincronizados();

			var parametroAnalista = _uowSciex.QueryStackSciex.ParametroAnalista1.Listar<ParametroAnalista1VM>();

			return AutoMapper.Mapper.Map<IEnumerable<ParametroAnalista1VM>>(parametroAnalista);
		}

		private void DataHoraSincronizarAnalista(int idAnalista)
		{
			var analista = _uowSciex.QueryStackSciex.Analista.Selecionar(x => x.IdAnalista == idAnalista);
			analista.DataHoraSincronizacao = DateTime.Now;
			_uowSciex.CommandStackSciex.Analista.Salvar(analista);
			_uowSciex.CommandStackSciex.Save();
		}

		private bool VerificarAnalistasNaoSincronizados()
		{
			var analistas = _uowSciex.QueryStackSciex.Analista.Listar();

			if (analistas != null)
			{
				foreach (var item in analistas)
				{
					//if (item.ParametroAnalista1.Count == 0)
					//{
					//	this.DataHoraSincronizarAnalista(item.IdAnalista);
					//	this.Salvar(InstanciarNovoParametroAnalista(item.IdAnalista));
					//}
					//else if ((item.DataHoraSincronizacao.ToShortDateString() != DateTime.Now.ToShortDateString())
					//	&& (item.DataHoraSincronizacao.ToShortTimeString() != DateTime.Now.ToShortTimeString()))
					//{
					//	foreach (var parametro in item.ParametroAnalista1)
					//	{
					//		this.Excluir(parametro.IdParametroAnalista);
					//	}
					//}
				}
			}
			return true;
		}

		private ParametroAnalista1VM InstanciarNovoParametroAnalista(int idAnalista)
		{
			ParametroAnalista1VM _ParametroAnalista1VM = new ParametroAnalista1VM();

			_ParametroAnalista1VM.IdAnalista = idAnalista;
			_ParametroAnalista1VM.StatusAnaliseVisual = 0;
			_ParametroAnalista1VM.DataAnaliseVisualInicio = DateTime.Now;
			_ParametroAnalista1VM.StatusAnaliseVisualBloqueio = 0;
			_ParametroAnalista1VM.StatusAnaliseLoteListagem = 0;
			_ParametroAnalista1VM.StatusAnaliseLoteListagemBloqueio = 0;
			_ParametroAnalista1VM.DescricaoAnaliseLoteListagemBloqueioFim = "";
			_ParametroAnalista1VM.DescricaoAnaliseVisualBloqueioFim = "";
			return _ParametroAnalista1VM;
		}

		public void Atualizar(ParametroAnalista1VM parametroAnalista1)
		{
			var parametroAnalista1Entity = _uowSciex.QueryStackSciex.ParametroAnalista1.Selecionar(x => x.IdParametroAnalista == parametroAnalista1.IdParametroAnalista);

			_uowSciex.CommandStackSciex.ParametroAnalista1.Salvar(parametroAnalista1Entity);
			_uowSciex.CommandStackSciex.Save();
		}

		public void Excluir(int idParametroAnalista)
		{
			_uowSciex.CommandStackSciex.ParametroAnalista1.Apagar(idParametroAnalista);
			_uowSciex.CommandStackSciex.Save();
		}

		public void Inserir(ParametroAnalista1VM parametroAnalista1)
		{
			var parametroAnalistaExistenteEntity = _uowSciex.QueryStackSciex.ParametroAnalista1.Selecionar(x =>
				x.Analista.CPF == parametroAnalista1.CPF
			);

			if (parametroAnalistaExistenteEntity != null)
			{
				throw new ValidationException(Resources.USUARIO_INTERNO_JA_INCLUIDO, new List<ValidationFailure> { new ValidationFailure("", Resources.USUARIO_INTERNO_JA_INCLUIDO) });
			}

			var parametroAnalista1Entity = Mapper.Map<ParametroAnalista1Entity>(parametroAnalista1);
			_uowSciex.CommandStackSciex.ParametroAnalista1.Salvar(parametroAnalista1Entity);
			_uowSciex.CommandStackSciex.Save();
		}

		public ParametroAnalista1VM Selecionar(int? idParametroAnalista)
		{
			var parametroAnalista1VM = new ParametroAnalista1VM();

			if (!idParametroAnalista.HasValue) { return parametroAnalista1VM; }

			var parametroAnalista = _uowSciex.QueryStackSciex.ParametroAnalista1.Selecionar(x => x.IdParametroAnalista == idParametroAnalista);

			if (parametroAnalista == null) { return parametroAnalista1VM; }

			parametroAnalista1VM = AutoMapper.Mapper.Map<ParametroAnalista1VM>(parametroAnalista);

			return parametroAnalista1VM;
		}

		public PagedItems<ParametroAnalista1VM> ListarPaginado(ParametroAnalista1VM parametroAnalista1)
		{
			if (VerificarAnalistasNaoSincronizados())
			{
				return _uowSciex.QueryStackSciex.ParametroAnalista1.ListarPaginado<ParametroAnalista1VM>(x =>
					(!parametroAnalista1.IdParametroAnalista.HasValue || x.IdParametroAnalista == parametroAnalista1.IdParametroAnalista)
				, parametroAnalista1);
			}
			return new PagedItems<ParametroAnalista1VM>();
		}

		public void RegrasSalvar(ParametroAnalista1VM parametroAnalista)
		{
			if (parametroAnalista == null) { return; }

			//var parametroAnalistaEntity = AutoMapper.Mapper.Map<ParametroAnalista1Entity>(parametroAnalista);
			ParametroAnalista1Entity parametroAnalistaEntity = new ParametroAnalista1Entity();			
			parametroAnalistaEntity.IdAnalista = parametroAnalista.IdAnalista;
			parametroAnalistaEntity.StatusAnaliseVisual = parametroAnalista.StatusAnaliseVisual;
			parametroAnalistaEntity.DataAnaliseVisualInicio = parametroAnalista.DataAnaliseVisualInicio;
			parametroAnalistaEntity.HoraAnaliseVisualInicio = parametroAnalista.HoraAnaliseVisualInicio;
			parametroAnalistaEntity.HoraAnaliseVisualFim = parametroAnalista.HoraAnaliseVisualFim;
			parametroAnalistaEntity.StatusAnaliseVisualBloqueio = parametroAnalista.StatusAnaliseVisualBloqueio;
			parametroAnalistaEntity.DataAnaliseVisualBloqueioInicio = parametroAnalista.DataAnaliseVisualBloqueioInicio;
			parametroAnalistaEntity.HoraAnaliseVisualBloqueioInicio = parametroAnalista.HoraAnaliseVisualBloqueioInicio;
			parametroAnalistaEntity.HoraAnaliseVisualBloqueioFim = parametroAnalista.HoraAnaliseVisualBloqueioFim;
			parametroAnalistaEntity.DescricaoAnaliseVisualBloqueioFim = parametroAnalista.DescricaoAnaliseVisualBloqueioFim;
			parametroAnalistaEntity.StatusAnaliseLoteListagem = parametroAnalista.StatusAnaliseLoteListagem;
			parametroAnalistaEntity.DataAnaliseLoteListagemInicio = parametroAnalista.DataAnaliseLoteListagemInicio;
			parametroAnalistaEntity.HoraAnaliseLoteListagemInicio = parametroAnalista.HoraAnaliseLoteListagemInicio;
			parametroAnalistaEntity.HoraAnaliseLoteListagemFim = parametroAnalista.HoraAnaliseLoteListagemFim;
			parametroAnalistaEntity.StatusAnaliseLoteListagemBloqueio = parametroAnalista.StatusAnaliseLoteListagemBloqueio;
			parametroAnalistaEntity.DataAnaliseListagemLoteBloqueioInicio = parametroAnalista.DataAnaliseListagemLoteBloqueioInicio;
			parametroAnalistaEntity.HoraAnaliseLoteListagemBloqueioInicio = parametroAnalista.HoraAnaliseLoteListagemBloqueioInicio;
			parametroAnalistaEntity.HoraAnaliseLoteListagemBloqueioFim = parametroAnalista.HoraAnaliseLoteListagemBloqueioFim;
			parametroAnalistaEntity.DescricaoAnaliseLoteListagemBloqueioFim = parametroAnalista.DescricaoAnaliseLoteListagemBloqueioFim;



			if (parametroAnalista.TipoSistema == 1)// 1: Analise Visual Bloquear   
			{

				parametroAnalistaEntity.DataAnaliseVisualBloqueioInicio = DateTime.Now;
				//parametroAnalistaEntity.DataAnaliseVisualInicio = parametroAnalista.DataAnaliseVisualInicio;

				// RN 14
				if (parametroAnalista.StatusAnaliseVisualBloqueio == 0)
				{
					parametroAnalistaEntity.HoraAnaliseVisualBloqueioInicio = null;
					parametroAnalistaEntity.HoraAnaliseVisualBloqueioFim = null;
				}
			}
			else if (parametroAnalista.TipoSistema == 2) // 2: Analise Listagem Bloquear
			{
				parametroAnalista.DataAnaliseListagemLoteBloqueioInicio = DateTime.Now;

				if (parametroAnalista.StatusAnaliseLoteListagemBloqueio == 0)
				{
					parametroAnalistaEntity.HoraAnaliseLoteListagemBloqueioInicio = null;
					parametroAnalistaEntity.HoraAnaliseLoteListagemBloqueioFim = null;
				}

			}
			else if (parametroAnalista.TipoSistema == 3) // 3: Analise Visual Ativar
			{
				//RN 11
				parametroAnalistaEntity.DataAnaliseVisualInicio = DateTime.Now;
				//parametroAnalistaEntity.DataAnaliseVisualBloqueioInicio = parametroAnalista.DataAnaliseVisualBloqueioInicio;
				


				// RN 12
				if (parametroAnalista.StatusAnaliseVisual == 0)
				{
					parametroAnalistaEntity.HoraAnaliseVisualInicio = null;
					parametroAnalistaEntity.HoraAnaliseVisualFim = null;
				}
			}
			else // 4: Analise Listagem Ativar
			{
				parametroAnalistaEntity.DataAnaliseLoteListagemInicio = DateTime.Now;

				if (parametroAnalista.StatusAnaliseLoteListagem == 0)
				{
					parametroAnalistaEntity.HoraAnaliseLoteListagemInicio = null;
					parametroAnalistaEntity.HoraAnaliseLoteListagemFim = null;
				}
			}





			_uowSciex.CommandStackSciex.ParametroAnalista1.Salvar(parametroAnalistaEntity);
		}

		public void Salvar(ParametroAnalista1VM parametroAnalista1VM)
		{
			RegrasSalvar(parametroAnalista1VM);
			_uowSciex.CommandStackSciex.Save();
		}
	}
}