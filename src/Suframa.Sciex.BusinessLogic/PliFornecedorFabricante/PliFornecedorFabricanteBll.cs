using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using Suframa.Sciex.BusinessLogic.Pss;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliFornecedorFabricanteBll : IPliFornecedorFabricanteBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;
		private readonly IComplementarPLIBll _complementarPLIBll;

		public PliFornecedorFabricanteBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uowCadsuf,
		 IViewImportadorBll viewImportadorBll, IComplementarPLIBll complementarPLIBll,
		 IUsuarioPssBll usuarioPssBll, IUsuarioInformacoesBll usuarioInformacoesBll)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_IViewImportadorBll = viewImportadorBll;
			_complementarPLIBll = complementarPLIBll;
			_usuarioPssBll = usuarioPssBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
		}

		public PliFornecedorFabricanteVM Selecionar(long idPliMercadoria)
		{
			var pliFornecedorFabricanteVM = new PliFornecedorFabricanteVM();

			var pliFornecedorFabricante = _uowSciex.QueryStackSciex.PliFornecedorFabricante.Selecionar(x => x.IdPliMercadoria == idPliMercadoria);
			if (pliFornecedorFabricante == null)
			{
				return null;
			}

			pliFornecedorFabricanteVM.IdPliMercadoria = pliFornecedorFabricante.IdPliMercadoria;
			pliFornecedorFabricanteVM.NumeroFabricante = pliFornecedorFabricante.NumeroFabricante;
			pliFornecedorFabricanteVM.NumeroFornecedor = pliFornecedorFabricante.NumeroFornecedor;
			pliFornecedorFabricanteVM.CodigoAusenciaFabricante = pliFornecedorFabricante.CodigoAusenciaFabricante;
			pliFornecedorFabricanteVM.CodigoPaisFabricante = pliFornecedorFabricante.CodigoPaisFabricante;
			pliFornecedorFabricanteVM.DescricaoLogradouroFornecedor = pliFornecedorFabricante.DescricaoLogradouroFornecedor;
			pliFornecedorFabricanteVM.CodigoPaisFornecedor = pliFornecedorFabricante.CodigoPaisFornecedor;
			pliFornecedorFabricanteVM.DescricaoComplementoFornecedor = pliFornecedorFabricante.DescricaoComplementoFornecedor;
			pliFornecedorFabricanteVM.DescricaoEstadoFornecedor = pliFornecedorFabricante.DescricaoEstadoFornecedor;
			pliFornecedorFabricanteVM.DescricaoCidadeFabricante = pliFornecedorFabricante.DescricaoCidadeFabricante;
			pliFornecedorFabricanteVM.DescricaoCidadeFornecedor = pliFornecedorFabricante.DescricaoCidadeFornecedor;
			pliFornecedorFabricanteVM.DescricaoComplementoFabricante = pliFornecedorFabricante.DescricaoComplementoFabricante;
			pliFornecedorFabricanteVM.DescricaoEstadoFabricante = pliFornecedorFabricante.DescricaoEstadoFabricante;
			pliFornecedorFabricanteVM.DescricaoFabricante = pliFornecedorFabricante.DescricaoFabricante;
			pliFornecedorFabricanteVM.DescricaoFornecedor = pliFornecedorFabricante.DescricaoFornecedor;
			pliFornecedorFabricanteVM.DescricaoLogradouroFabricante = pliFornecedorFabricante.DescricaoLogradouroFabricante;
			pliFornecedorFabricanteVM.DescricaoPaisFabricante = pliFornecedorFabricante.DescricaoPaisFabricante;
			pliFornecedorFabricanteVM.DescricaoPaisFornecedor = pliFornecedorFabricante.DescricaoPaisFornecedor;
			pliFornecedorFabricanteVM.CodigoDescricaoPaisFornecedorConcatenado = pliFornecedorFabricante.CodigoPaisFornecedor + " | " + pliFornecedorFabricante.DescricaoPaisFornecedor;
			pliFornecedorFabricanteVM.CodigoDescricaoPaisFabricanteConcatenado = pliFornecedorFabricante.CodigoPaisFabricante + " | " + pliFornecedorFabricante.DescricaoPaisFabricante;
			return pliFornecedorFabricanteVM;
		}


	}





}