using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Suframa.Sciex.BusinessLogic
{
	public class ControleImportacaoBll : IControleImportacaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uow;
		private readonly Validation _validation;

		public ControleImportacaoBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uow)
		{
			_uowSciex = uowSciex;
			_uow = uow;
			_validation = new Validation();
		}

		public IEnumerable<ControleImportacaoVM> Listar(ControleImportacaoVM controleImportacaoVM)
		{
			var controleImportacao = _uowSciex.QueryStackSciex.ControleImportacao.Listar<ControleImportacaoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<ControleImportacaoVM>>(controleImportacao);
		}

		public IEnumerable<object> ListarSetor()
		{
			return _uow.QueryStack.Setor
				.Listar()
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.IdSetor,
						text = s.Descricao
					});
		}

		public PagedItems<ControleImportacaoVM> ListarPaginado(ControleImportacaoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<ControleImportacaoVM>(); }

			var controleImportacao = _uowSciex.QueryStackSciex.ControleImportacao.ListarPaginado<ControleImportacaoVM>(o =>
				(
					(
						pagedFilter.IdPliAplicacao == 0 || o.IdPliAplicacao == pagedFilter.IdPliAplicacao
					) &&
					(
						pagedFilter.CodigoSetor == 0 || o.CodigoSetor == pagedFilter.CodigoSetor
					) &&
					(
						pagedFilter.IdCodigoConta == 0 || o.IdCodigoConta == pagedFilter.IdCodigoConta
					) &&
					(
						pagedFilter.IdCodigoUtilizacao == 0 || o.IdCodigoUtilizacao == pagedFilter.IdCodigoUtilizacao
					) &&
					(
						pagedFilter.Status == 2 || o.Status == pagedFilter.Status
					)
				),
				pagedFilter);

			return controleImportacao;
		}

		//Verifica Duplicação
		public bool verificaDuplicacao(ControleImportacaoEntity controleImportacao)
		{
			var controleImportacaoEntity = _uowSciex.QueryStackSciex.ControleImportacao.Listar

				(x => x.PliAplicacao.IdPliAplicacao == controleImportacao.IdPliAplicacao &&
					  x.CodigoSetor == controleImportacao.CodigoSetor &&
					  x.CodigoConta.IdCodigoConta == controleImportacao.IdCodigoConta &&
					  x.IdCodigoUtilizacao == controleImportacao.IdCodigoUtilizacao );

			if (controleImportacaoEntity.Count > 0)
				return true;

			return false;
		}

		public bool verificaVinculo(ControleImportacaoVM controleImportacao)
		{
			if (controleImportacao.Ativar == 1)
			{
				var controleImportacaoEntity = _uowSciex.QueryStackSciex.ControleImportacao.Listar(
					(x => x.IdCodigoConta == controleImportacao.IdCodigoConta && x.CodigoConta.Status == 1 &&
					 x.IdCodigoUtilizacao == controleImportacao.IdCodigoUtilizacao && x.CodigoUtilizacao.Status == 1));

				if (controleImportacaoEntity.Count == 0)
					return true;

			}
			return false;
		}


		public ControleImportacaoVM RegrasSalvar(ControleImportacaoVM controleImportacao)
		{
			if (controleImportacao == null) { return null; }

			// Salva ControleImportacao
			var controleImportacaoEntity = AutoMapper.Mapper.Map<ControleImportacaoEntity>(controleImportacao);

			if (controleImportacaoEntity == null) { return null; }

			//Verifica Vínculo
			if (verificaVinculo(controleImportacao))
			{
				controleImportacao.MensagemErroVinculo = "Existem itens associados que estão inativos";
				return controleImportacao;
			}
			controleImportacao.MensagemErroVinculo = null;


			if (verificaDuplicacao(controleImportacaoEntity) && controleImportacao.isEditStatus != 1)
			{
				controleImportacao.MensagemErro = "Registro já existe";
				return controleImportacao;
			}
				controleImportacao.MensagemErro = null;

			if (controleImportacao.IdControleImportacao.HasValue)
			{
				controleImportacaoEntity = _uowSciex.QueryStackSciex.ControleImportacao.Selecionar(x => x.IdControleImportacao == controleImportacao.IdControleImportacao);

				controleImportacaoEntity = AutoMapper.Mapper.Map(controleImportacao, controleImportacaoEntity);
			}

			_uowSciex.CommandStackSciex.ControleImportacao.Salvar(controleImportacaoEntity);

			return controleImportacao;
		}

		public void Salvar(ControleImportacaoVM controleImportacaoVM)
		{
			controleImportacaoVM = RegrasSalvar(controleImportacaoVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public ControleImportacaoVM Selecionar(int? idControleImportacao)
		{
			var controleImportacaoVM = new ControleImportacaoVM();

			if (!idControleImportacao.HasValue) { return controleImportacaoVM; }

			var controleImportacao = _uowSciex.QueryStackSciex.ControleImportacao.Selecionar(x => x.IdControleImportacao == idControleImportacao);

			//if (controleImportacao == null)
			//{
			//	_validation._controleImportacaoExcluirValidation.ValidateAndThrow(new ControleImportacaoDto
			//	{
			//		ExisteRegistro = false
			//	});
			//}

			controleImportacaoVM = AutoMapper.Mapper.Map<ControleImportacaoVM>(controleImportacao);

			return controleImportacaoVM;
		}

		public void Deletar(int id)
		{
			var controleImportacao = _uowSciex.QueryStackSciex.ControleImportacao.Selecionar(s => s.IdControleImportacao == id);

			//if (controleImportacao != null)
			//{
			//	_validation._controleImportacaoExisteRelacionamentoValidation.ValidateAndThrow(new ControleImportacaoDto
			//	{
			//		TotalEncontradoControleImportacao = controleImportacao.Parametros.Count,
			//	});
			//}
			//else
			//{
			//	_validation._controleImportacaoExcluirValidation.ValidateAndThrow(new ControleImportacaoDto
			//	{
			//		ExisteRegistro = false
			//	});
			//}

			if (controleImportacao != null)
			{
				_uowSciex.CommandStackSciex.ControleImportacao.Apagar(controleImportacao.IdControleImportacao);
			}

			_uowSciex.CommandStackSciex.Save();
		}
	}
}