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
	public class PliProdutoBll : IPliProdutoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		public PliProdutoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public IEnumerable<PliProdutoVM> Listar(PliProdutoVM pliProdutoVM)
		{
			var pliProduto = _uowSciex.QueryStackSciex.PliProduto.Listar(o => o.IdPLI == pliProdutoVM.IdPLI);
			return AutoMapper.Mapper.Map<IEnumerable<PliProdutoVM>>(pliProduto);
		}


		public PagedItems<PliProdutoVM> ListarPaginado(PliProdutoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<PliProdutoVM>(); }


			var pliProduto = _uowSciex.QueryStackSciex.PliProduto.ListarPaginado<PliProdutoVM>(o =>
				(
					(
						pagedFilter.IdPLI == 0 || o.IdPLI == pagedFilter.IdPLI
					)
				),
				pagedFilter);

			return pliProduto;
		}

		public PliProdutoVM RegrasSalvar(PliProdutoVM pliProduto)
		{
			if (pliProduto == null) { return null; }

			// Salva PliProduto
			var pliProdutoEntity = AutoMapper.Mapper.Map<PliProdutoEntity>(pliProduto);

			if (pliProdutoEntity == null) { return null; }

			if (pliProduto.IdPliProduto.HasValue)
			{
				pliProdutoEntity = _uowSciex.QueryStackSciex.PliProduto.Selecionar(x => x.IdPliProduto == pliProduto.IdPliProduto);

				pliProdutoEntity = AutoMapper.Mapper.Map(pliProduto, pliProdutoEntity);
			}
			else
			{
				var viewProdutoEmpresaVM = ConsultaProdutoMercadoria(pliProduto.IdProdutoEmpresa);

				pliProdutoEntity.CodigoModeloProduto = viewProdutoEmpresaVM.CodigoModeloProduto;
				pliProdutoEntity.CodigoProduto = viewProdutoEmpresaVM.CodigoProduto;
				pliProdutoEntity.CodigoTipoProduto = viewProdutoEmpresaVM.CodigoTipoProduto;
				pliProdutoEntity.Descricao = viewProdutoEmpresaVM.Descricao;

				var pliProdutoValida = _uowSciex.QueryStackSciex.PliProduto
					.Listar(o => o.IdPLI == pliProduto.IdPLI && o.CodigoProduto == viewProdutoEmpresaVM.CodigoProduto && o.CodigoModeloProduto == viewProdutoEmpresaVM.CodigoModeloProduto && o.CodigoTipoProduto == viewProdutoEmpresaVM.CodigoTipoProduto);

				pliProduto.MensagemErro = "";
				if (pliProdutoValida.Count != 0)
				{
					pliProduto.MensagemErro = "O produto selecionado já foi cadastrado.";
					return pliProduto;
				}

			}

			_uowSciex.CommandStackSciex.PliProduto.Salvar(pliProdutoEntity);

			return pliProduto;
		}

		private ViewProdutoEmpresaVM ConsultaProdutoMercadoria(int IdProdutoEmpresa)
		{
			var viewProdutoEmpresaEntity = _uowSciex.QueryStackSciex.ViewProdutoEmpresa.Selecionar(o => o.IdProdutoEmpresa == IdProdutoEmpresa);

			return AutoMapper.Mapper.Map<ViewProdutoEmpresaVM>(viewProdutoEmpresaEntity);

		}


		public PliProdutoVM Salvar(PliProdutoVM pliProdutoVM)
		{
			pliProdutoVM = RegrasSalvar(pliProdutoVM);
			_uowSciex.CommandStackSciex.Save();
			return pliProdutoVM;

		}

		public PliProdutoVM AtualizarProduto(PliProdutoVM pliProdutoVM)
		{
			if (pliProdutoVM == null) { return null; }

			// Salva PliProduto
			var pliProdutoEntity = AutoMapper.Mapper.Map<PliProdutoEntity>(pliProdutoVM);

			if (pliProdutoEntity == null) { return null; }

			var pliProdutoOldEntity = _uowSciex.QueryStackSciex.PliProduto.Selecionar(o => o.IdPliProduto == pliProdutoVM.IdPliProduto);

			var viewProdutoNovo = ConsultaProdutoMercadoria(pliProdutoVM.IdProdutoEmpresa);

			//valida1
			var pliProdutoValida = _uowSciex.QueryStackSciex.PliProduto
				.Listar(o => o.IdPLI == pliProdutoVM.IdPLI && o.CodigoProduto == viewProdutoNovo.CodigoProduto && o.CodigoModeloProduto == viewProdutoNovo.CodigoModeloProduto && o.CodigoTipoProduto == viewProdutoNovo.CodigoTipoProduto);

			pliProdutoVM.MensagemErro = "";
			if (pliProdutoValida.Count != 0)
			{
				pliProdutoVM.MensagemErro = "O produto selecionado já foi cadastrado.";
				return pliProdutoVM;
			}
			// valida 1 fim

			var existeNcm = _uowSciex.QueryStackSciex.PliMercadoria.Listar(o => o.IdPLI == pliProdutoVM.IdPLI && o.IdPliProduto == pliProdutoOldEntity.IdPliProduto);
			List<ViewMercadoriaEntity> list = new List<ViewMercadoriaEntity>();

			foreach (var item in existeNcm)
			{
				var ncm = _uowSciex.QueryStackSciex.ViewMercadoria.Selecionar(o => o.CodigoNCMMercadoria == item.CodigoNCMMercadoria && o.CodigoProdutoMercadoria == viewProdutoNovo.CodigoProduto && o.StatusMercadoria == 1);
				if(ncm != null)
					list.Add(ncm);
			}

			if (existeNcm.Count != 0 && list.Count == 0)
			{
				pliProdutoVM.MensagemErro = "O produto selecionado deve corresponder a(s) NCM(s) já cadastrada(s), do contrário não será adicionado.";
				return pliProdutoVM;
			}

			pliProdutoOldEntity.CodigoModeloProduto = viewProdutoNovo.CodigoModeloProduto;
			pliProdutoOldEntity.CodigoProduto = viewProdutoNovo.CodigoProduto;
			pliProdutoOldEntity.CodigoTipoProduto = viewProdutoNovo.CodigoTipoProduto;
			pliProdutoOldEntity.Descricao = viewProdutoNovo.Descricao;

			_uowSciex.CommandStackSciex.PliProduto.Salvar(pliProdutoOldEntity);
			_uowSciex.CommandStackSciex.Save();

			return pliProdutoVM;
		}

		public PliProdutoVM Selecionar(int? idPliProduto)
		{
			var pliProdutoVM = new PliProdutoVM();

			if (!idPliProduto.HasValue) { return pliProdutoVM; }

			var pliProduto = _uowSciex.QueryStackSciex.PliProduto.Selecionar(x => x.IdPliProduto == idPliProduto);

			pliProdutoVM = AutoMapper.Mapper.Map<PliProdutoVM>(pliProduto);

			return pliProdutoVM;
		}

		public void Deletar(long id)
		{

			var pliProduto = _uowSciex.QueryStackSciex.PliProduto.Selecionar(s => s.IdPliProduto == id);

			if (pliProduto != null)
			{
				_uowSciex.CommandStackSciex.PliProduto.Apagar(pliProduto.IdPliProduto);
			}

			_uowSciex.CommandStackSciex.Save();


		}
	}
}