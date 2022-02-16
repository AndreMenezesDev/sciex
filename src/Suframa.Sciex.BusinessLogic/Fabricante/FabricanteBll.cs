using AutoMapper;
using FluentValidation;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic
{
	public class FabricanteBll : IFabricanteBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;
		private readonly IUsuarioLogado _usuarioLogado;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IUsuarioPssBll _usuarioPssBll;

		private string CNPJ { get; set; }

		public FabricanteBll(IUnitOfWorkSciex uowSciex,
			IUsuarioLogado usuarioLogadoBll, IUsuarioInformacoesBll usuarioInformacoesBll, IUsuarioPssBll usuarioPssBll)
		{
			_usuarioPssBll = usuarioPssBll;
			_uowSciex = uowSciex;
			_usuarioLogado = usuarioLogadoBll;
			_validation = new Validation();
			_usuarioInformacoesBll = usuarioInformacoesBll;
			this.CNPJ = _usuarioInformacoesBll.ObterCNPJ().CnpjCpfUnformat();
		}

		public IEnumerable<FabricanteVM> Listar(FabricanteVM fabricanteVM)
		{
			var fabricante = _uowSciex.QueryStackSciex.Fabricante.Listar<FabricanteVM>();
			return AutoMapper.Mapper.Map<IEnumerable<FabricanteVM>>(fabricante);
		}

		public IEnumerable<object> ListarChave(FabricanteVM fabricanteVM)
		{
			if (fabricanteVM.Descricao == null && fabricanteVM.Id == null)
			{
				return new List<object>();
			}
			try
			{
				string descricao = fabricanteVM.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				int valor = Convert.ToInt32(fabricanteVM.Descricao);
				fabricanteVM.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.Fabricante
				.Listar().Where(o =>
						(fabricanteVM.Descricao == null || (o.RazaoSocial.ToLower().Contains(fabricanteVM.Descricao.ToLower()) || o.Codigo.ToString().Contains(fabricanteVM.Descricao.ToString())))
					&&
						(fabricanteVM.Id == null || o.IdFabricante == fabricanteVM.Id)
					&&
						(fabricanteVM.CNPJImportador == null || o.CNPJImportador == fabricanteVM.CNPJImportador.CnpjCpfUnformat())
					)
				.OrderBy(o => o.DescricaoPais)
				.Select(
					s => new
					{
						id = s.IdFabricante,
						pais = s.CodigoPais + " | " + s.DescricaoPais,
						logradouro = s.Logradouro,
						complemento = s.Complemento,
						numero = s.Numero,
						cidade = s.Cidade,
						estado = s.Estado,
						text = s.Codigo.ToString("D2") + " | " + s.RazaoSocial
					}).Where(o => o.text.Contains(fabricanteVM.Descricao.ToString())); 
				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.Fabricante
					.Listar().Where(o =>
							(fabricanteVM.Descricao == null || (o.RazaoSocial.ToLower().Contains(fabricanteVM.Descricao.ToLower()) || o.Codigo.ToString().Contains(fabricanteVM.Descricao.ToString())))
						&&
							(fabricanteVM.Id == null || o.IdFabricante == fabricanteVM.Id)
						&&
							(fabricanteVM.CNPJImportador == null || o.CNPJImportador == fabricanteVM.CNPJImportador.CnpjCpfUnformat())
						)
					.OrderBy(o => o.DescricaoPais)
					.Select(
						s => new
						{
							id = s.IdFabricante,
							pais = s.CodigoPais + " | " + s.DescricaoPais,
							logradouro = s.Logradouro,
							complemento = s.Complemento,
							numero = s.Numero,
							cidade = s.Cidade,
							estado = s.Estado,
							text = s.Codigo.ToString("D2") + " | " + s.RazaoSocial
						});
				return lista;
			}

		}

		public PagedItems<FabricanteVM> ListarPaginado(FabricanteVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<FabricanteVM>(); }


			var fabricante = _uowSciex.QueryStackSciex.Fabricante.ListarPaginado<FabricanteVM>(o =>
				(
					//(
					//	pagedFilter.IdFabricante == -1 || o.IdFabricante == pagedFilter.IdFabricante
					//) &&
					(
						pagedFilter.Codigo == -1 || o.Codigo == pagedFilter.Codigo
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.RazaoSocial) ||
						o.RazaoSocial.Contains(pagedFilter.RazaoSocial)
					) &&
					(
						o.CNPJImportador == this.CNPJ
					)
				),
				pagedFilter);

			return fabricante;
		}

		public FabricanteVM RegrasSalvar(FabricanteVM fabricanteVM)
		{
			var Cnpj = this.CNPJ;
			if (fabricanteVM.Codigo == 0 || fabricanteVM.Codigo == null)
			{
				var fabricante = _uowSciex.QueryStackSciex.Fabricante.Listar<FabricanteVM>(o => o.CNPJImportador == CNPJ);

				if (fabricante.Count == 0)
				{
					fabricanteVM.Codigo = 1;
				}
				else
				{
					var codigo = fabricante.LastOrDefault().Codigo;
					fabricanteVM.Codigo = 0;
					fabricanteVM.Codigo = codigo + 1;
				}
			}

			var entityFabricante = AutoMapper.Mapper.Map<FabricanteEntity>(fabricanteVM);

			entityFabricante.CNPJImportador = this.CNPJ;

			_uowSciex.CommandStackSciex.Fabricante.Salvar(entityFabricante);

			_uowSciex.CommandStackSciex.Save();

			var _fabricanteVM = AutoMapper.Mapper.Map<FabricanteVM>(entityFabricante);

			return _fabricanteVM;
		}

		public FabricanteVM Salvar(FabricanteVM fabricanteVM)
		{
			return RegrasSalvar(fabricanteVM);
		}

		public FabricanteVM Visualizar(FabricanteVM fabricanteVM)
		{
			var entity = _uowSciex.QueryStackSciex.Fabricante.Selecionar(x => x.IdFabricante == fabricanteVM.IdFabricante);

			var retorno = AutoMapper.Mapper.Map<FabricanteVM>(entity);

			return retorno;
		}

		public FabricanteVM Selecionar(int? idFabricante)
		{

			var fabricanteVM = new FabricanteVM();

			if (!idFabricante.HasValue) { return fabricanteVM; }

			var fabricante = _uowSciex.QueryStackSciex.Fabricante.Selecionar(x => x.IdFabricante == idFabricante);

			if (fabricante == null) { return fabricanteVM; }

			fabricanteVM = AutoMapper.Mapper.Map<FabricanteVM>(fabricante);

			return fabricanteVM;
		}

		public void Deletar(int id)
		{
			var fabricante = _uowSciex.QueryStackSciex.Fabricante.Selecionar(s => s.IdFabricante == id);

			if (fabricante != null)
			{

				_validation._fabricanteExisteRelacionamentoValidation.ValidateAndThrow(new FabricanteDto
				{
					TotalEncontradoFabricante = fabricante.Parametros.Count,

				});

				_validation._fabricanteExisteRelacionamentoValidation.ValidateAndThrow(new FabricanteDto
				{
					TotalEncontradoFabricante = fabricante.PliMercadoria.Count,

				});

			}
			else
			{
				_validation._fabricanteExcluirValidation.ValidateAndThrow(new FabricanteDto
				{
					ExisteRegistro = false
				});
			}


			if (fabricante != null)
			{
				_uowSciex.CommandStackSciex.Fabricante.Apagar(fabricante.IdFabricante);
			}

			_uowSciex.CommandStackSciex.Save();


		}
	}
	
}