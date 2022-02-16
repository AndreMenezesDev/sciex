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

namespace Suframa.Sciex.BusinessLogic
{
	public class FornecedorBll : IFornecedorBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;
		private readonly IUsuarioLogado _usuarioLogado;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private string CNPJ { get; set; }

		public FornecedorBll(IUnitOfWorkSciex uowSciex,
			IUsuarioLogado usuarioLogado, IUsuarioInformacoesBll usuarioInformacoesBll)
		{
			_uowSciex = uowSciex;
			_usuarioLogado = usuarioLogado;
			_validation = new Validation();
			_usuarioInformacoesBll = usuarioInformacoesBll;
			this.CNPJ = _usuarioInformacoesBll.ObterCNPJ().CnpjCpfUnformat();
		}

		private void ExclusaoLogica(FornecedorVM fornecedorVM)
		{
			fornecedorVM.Ativo = 0;
			RegrasSalvar(fornecedorVM);
		}

		public void Deletar(int id)
		{
			var fornecedor = _uowSciex.QueryStackSciex.Fornecedor.Selecionar(s => s.IdFornecedor == id);

			if (fornecedor != null)
			{

				_validation._fornecedorExisteRelacionamentoValidation.ValidateAndThrow(new FornecedorDto
				{
					TotalEncontradoFornecedor = fornecedor.Parametros.Count,

				});

				_validation._fornecedorExisteRelacionamentoValidation.ValidateAndThrow(new FornecedorDto
				{
					TotalEncontradoFornecedor = fornecedor.PliMercadoria.Count,

				});

			}
			else
			{
				_validation._fornecedorExcluirValidation.ValidateAndThrow(new FornecedorDto
				{
					ExisteRegistro = false
				});
			}


			if (fornecedor != null)
			{
				_uowSciex.CommandStackSciex.Fornecedor.Apagar(fornecedor.IdFornecedor);
			}

			_uowSciex.CommandStackSciex.Save();
		}

		public IEnumerable<FornecedorVM> Listar(FornecedorVM FornecedorVM)
		{
			var Fornecedor = _uowSciex.QueryStackSciex.Fornecedor.Listar<FornecedorVM>();

			return AutoMapper.Mapper.Map<IEnumerable<FornecedorVM>>(Fornecedor);
		}

		public IEnumerable<object> ListarChave(FornecedorVM fornecedorVM)
		{

			if (fornecedorVM.Descricao == null && fornecedorVM.Id == null)
			{
				return new List<object>();
			}
			try
			{
				string descricao = fornecedorVM.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				int valor = Convert.ToInt32(fornecedorVM.Descricao);
				fornecedorVM.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.Fornecedor
				.Listar().Where(o =>
						(fornecedorVM.Descricao == null || (o.RazaoSocial.ToLower().Contains(fornecedorVM.Descricao.ToLower()) || o.Codigo.ToString().Contains(fornecedorVM.Descricao.ToString())))
					&&
						(fornecedorVM.Id == null || o.IdFornecedor == fornecedorVM.Id)
					&&
						(fornecedorVM.CNPJImportador == null || o.CNPJImportador == fornecedorVM.CNPJImportador.CnpjCpfUnformat())
					)
				.OrderBy(o => o.DescricaoPais).Select(
					s => new
					{
						id = s.IdFornecedor,
						pais = s.CodigoPais + " | " + s.DescricaoPais,
						logradouro = s.Logradouro,
						complemento = s.Complemento,
						numero = s.Numero,
						cidade = s.Cidade,
						estado = s.Estado,
						text = s.Codigo.ToString("D2") + " | " + s.RazaoSocial
					}).Where(o=>o.text.Contains(fornecedorVM.Descricao.ToString()));
				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.Fornecedor
					.Listar().Where(o =>
							(fornecedorVM.Descricao == null || (o.RazaoSocial.ToLower().Contains(fornecedorVM.Descricao.ToLower()) || o.Codigo.ToString().Contains(fornecedorVM.Descricao.ToString())))
						&&
							(fornecedorVM.Id == null || o.IdFornecedor == fornecedorVM.Id)
						&&
							(fornecedorVM.CNPJImportador == null || o.CNPJImportador == fornecedorVM.CNPJImportador.CnpjCpfUnformat())	
						)
					.OrderBy(o => o.DescricaoPais)
					.Select(
						s => new
						{
							id = s.IdFornecedor,
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

		public PagedItems<FornecedorVM> ListarPaginado(FornecedorVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<FornecedorVM>(); }


			var fornecedor = _uowSciex.QueryStackSciex.Fornecedor.ListarPaginado<FornecedorVM>(o =>
				(
					//(
					//	pagedFilter.IdFornecedor == -1 || o.IdFornecedor == pagedFilter.IdFornecedor
					//) &&
					(
						pagedFilter.Codigo == -1 || o.Codigo == pagedFilter.Codigo
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.RazaoSocial) ||
						o.RazaoSocial.Contains(pagedFilter.RazaoSocial)
					)&&
					(
						o.CNPJImportador == this.CNPJ
					)
				),
				pagedFilter);

			return fornecedor;
		}

		public FornecedorVM RegrasSalvar(FornecedorVM fornecedorVM)
		{
			var Cnpj = CNPJ;
			if (fornecedorVM.Codigo == 0 || fornecedorVM.Codigo == null)
			{
				var fornecedor = _uowSciex.QueryStackSciex.Fornecedor.Listar<FornecedorVM>(o => o.CNPJImportador == CNPJ);

				if (fornecedor.Count == 0)
				{
					fornecedorVM.Codigo = 1;
				}
				else
				{
					var codigo = fornecedor.LastOrDefault().Codigo;
					fornecedorVM.Codigo = 0;
					fornecedorVM.Codigo = codigo + 1;
				}
			}
			var entityFornecedor = AutoMapper.Mapper.Map<FornecedorEntity>(fornecedorVM);
			
			//entityFornecedor.CNPJImportador = _IUsuarioLogado.Usuario.CpfCnpj;
			entityFornecedor.CNPJImportador = this.CNPJ;

			_uowSciex.CommandStackSciex.Fornecedor.Salvar(entityFornecedor);

			_uowSciex.CommandStackSciex.Save();

			var _fornecedorVM = AutoMapper.Mapper.Map<FornecedorVM>(entityFornecedor);

			return _fornecedorVM;
		}

		public FornecedorVM Salvar(FornecedorVM fornecedorVM)
		{
			return RegrasSalvar(fornecedorVM);
		}

		public FornecedorVM Selecionar(int? idFornecedor)
		{
			var FornecedorVM = new FornecedorVM();

			if (!idFornecedor.HasValue) { return FornecedorVM; }

			var Fornecedor = _uowSciex.QueryStackSciex.Fornecedor.Selecionar(x => x.IdFornecedor == idFornecedor);

			if (Fornecedor == null) { return FornecedorVM; }

			FornecedorVM = AutoMapper.Mapper.Map<FornecedorVM>(Fornecedor);

			return FornecedorVM;
		}
	}
}