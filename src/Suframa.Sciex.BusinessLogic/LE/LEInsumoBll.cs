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
using System.Text;
using Suframa.Sciex.CrossCutting.Extension;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls.Expressions;
using System.Linq.Expressions;

namespace Suframa.Sciex.BusinessLogic
{
	public class LEInsumoBll : ILEInsumoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;

		public LEInsumoBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uowCadsuf,
			 IViewImportadorBll viewImportadorBll, IComplementarPLIBll complementarPLIBll,
			 IUsuarioPssBll usuarioPssBll, IUsuarioInformacoesBll usuarioInformacoesBll)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_IViewImportadorBll = viewImportadorBll;
			_usuarioPssBll = usuarioPssBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
		}

		public IEnumerable<LEInsumoVM> Listar(LEInsumoVM vm)
		{
			var leInsumo = _uowSciex.QueryStackSciex.LEInsumo.Listar<LEInsumoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<LEInsumoVM>>(leInsumo);
		}

		public IEnumerable<object> Listar()
		{
			return _uowSciex.QueryStackSciex.Pli
				.Listar()
				.OrderBy(o => o.NumeroPli)
				.Select(
					s => new
					{
						id = s.IdPLI,
						text = s.CodigoCNAE + " - " + s.NumeroPli
					});
		}

		public PagedItems<LEInsumoVM> ListarPaginado(LEInsumoVM pagedFilter)
		{
			pagedFilter.CodigoProdutoSuframa = Convert.ToInt16(pagedFilter.DescricaoCodigoProdutoSuframa.Substring(0, 4));

			#region
			var cancelado = 4;
			#endregion



			if (pagedFilter == null) { return new PagedItems<LEInsumoVM>(); }

			if (pagedFilter.Sort == null)
				pagedFilter.Sort = "CodigoInsumo";

			#region TIPO INSUMOS
			var arraytipoInsumo = new List<string>();
			var TODOS = "99";
			if (pagedFilter.TipoInsumo == TODOS || pagedFilter.TipoInsumo == null)
			{
				arraytipoInsumo.Add("P"); //P - Insumo Padrão
				arraytipoInsumo.Add("E"); //E - Insumo Estrangeiro Extra Listagem Padrão
				arraytipoInsumo.Add("N"); //N - Nacional
				arraytipoInsumo.Add("R"); //R - Regional
			}
			else
			{
				arraytipoInsumo.Add(pagedFilter.TipoInsumo);
			}
			#endregion

			#region SITUAÇÃO INSUMO
			var arraySitucaoInsumo = new List<int?>();
			var _todos = 99;


			#endregion

			Expression<Func<LEInsumoVM, bool>> predicado;
			if (pagedFilter.IsAlteracao)
			{
				if (pagedFilter.SituacaoInsumo == _todos)
				{
					arraySitucaoInsumo.Add(null); //nula/ao criar cópia
					arraySitucaoInsumo.Add(1); //Entregue
					arraySitucaoInsumo.Add(2); //Aguardando Aprovação
					arraySitucaoInsumo.Add(3); //Aprovado
				}
				else
				{
					arraySitucaoInsumo.Add(pagedFilter.SituacaoInsumo);
				}
				predicado = o =>
				(
					(
						pagedFilter.IdLe == 0 || o.IdLe.Equals(pagedFilter.IdLe)
					)
					&&
					(
						pagedFilter.CodigoNCM == null || o.CodigoNCM.Equals(pagedFilter.CodigoNCM)
					)
					&&
					( 
						arraytipoInsumo.Contains(o.TipoInsumo)
					)
					&&
					(
						pagedFilter.CodigoInsumo == 0 || o.CodigoInsumo.Equals(pagedFilter.CodigoInsumo)
					)
					&&
					(
						arraySitucaoInsumo.Contains(o.SituacaoInsumo)
					)
					&&
					(
						o.TipoInsumoAlteracao > 0
					)
				);

			}
			else
			{
				if (pagedFilter.SituacaoInsumo == _todos)
				{
					arraySitucaoInsumo.Add(null); //nula/ao criar cópia
					arraySitucaoInsumo.Add(1); //Entregue
					arraySitucaoInsumo.Add(2); //Aguardando Aprovação
					//arraySitucaoInsumo.Add(3); //Aprovado
				}
				else
				{
					arraySitucaoInsumo.Add(pagedFilter.SituacaoInsumo);
				}
				int Bloqueado = 2;

				var listaInsumosCopiaDeBloqueados = _uowSciex.QueryStackSciex.LEInsumo.Listar(o =>
																					o.IdLe == pagedFilter.IdLe
																					&&
																					o.SituacaoInsumo == null
																					).Select(q => q.CodigoInsumo).ToList();

				var listaInsumosBloqueadosParaNaoExibir = _uowSciex.QueryStackSciex.LEInsumo.Listar(o =>
																					o.IdLe == pagedFilter.IdLe
																					&&
																					listaInsumosCopiaDeBloqueados.Contains(o.CodigoInsumo)
																					&& 
																					o.SituacaoInsumo == Bloqueado
																					).Select(q=> q.IdLeInsumo).ToList();

				


				predicado = o =>
				(
					(
						pagedFilter.IdLe == 0 || o.IdLe.Equals(pagedFilter.IdLe)
					)
					&&					
					(
						pagedFilter.CodigoNCM == null || o.CodigoNCM.Equals(pagedFilter.CodigoNCM)
					)
					&&
					(
						arraytipoInsumo.Contains(o.TipoInsumo)
					)					
					&&
					(
						pagedFilter.CodigoInsumo == 0 || o.CodigoInsumo.Equals(pagedFilter.CodigoInsumo)
					)
					&&
					(
						arraySitucaoInsumo.Contains(o.SituacaoInsumo)
					)	
					&&
					(
						!listaInsumosBloqueadosParaNaoExibir.Contains(o.IdLeInsumo)
					)
				);
			}

			var insumos = _uowSciex.QueryStackSciex.LEInsumo.ListarPaginadoGrafo(q=> new LEInsumoVM()
				{
				IdLe = q.IdLe,
				IdLeInsumo = q.IdLeInsumo,
				SituacaoInsumo = q.SituacaoInsumo,
				CodigoProduto = q.CodigoProduto,
				CodigoInsumo = q.CodigoInsumo,
				CodigoNCM = q.CodigoNCM,
				TipoInsumo = q.TipoInsumo,
				TipoInsumoAlteracao = q.TipoInsumoAlteracao != null ? (int)q.TipoInsumoAlteracao : (int)0,
				CodigoUnidadeMedida = q.CodigoUnidadeMedida,
				CodigoDetalhe = q.CodigoDetalhe,
				DescricaoInsumo = q.DescricaoInsumo,
				ValorCoeficienteTecnico = q.ValorCoeficienteTecnico,
				CodigoPartNumber = q.CodigoPartNumber,
				DescricaoEspecTecnica = q.DescricaoEspecTecnica,
				listaInsumoErro = q.listaLEInsumoErro.Select(w => new LEInsumoErroVM()
									{
										IdLeInsumo = w.LEInsumo.IdLeInsumo,
										IdLeInsumoErro = w.IdLeInsumoErro,
										CpfResponsavel = w.CpfResponsavel,
										DataErroRegistro = w.DataErroRegistro,
										DescricaoErro = w.DescricaoErro,
										NomeResponsavel = w.NomeResponsavel,
									}).ToList()
				},
				predicado, pagedFilter);

			foreach (var item in insumos.Items)
			{

				if(item.listaInsumoErro != null && item.listaInsumoErro.Count > 0)
				{
					item.UltimoInsumoErro = item.listaInsumoErro.LastOrDefault();
				}

				//item.DescricaoTipoInsumo = item.TipoInsumo == "1" ? "P" : item.TipoInsumo == "2" ? "E" : item.TipoInsumo == "3" ? "N" : item.TipoInsumo == "4" ? "R" : " ";
				item.DescricaoTipoInsumo = item.TipoInsumo;
				item.DescricaoTipoInsumoAlteracao = item.TipoInsumoAlteracao == 1 ? "Novo" 
													: item.TipoInsumoAlteracao == 2 ? "Alterado" 
													: item.TipoInsumoAlteracao == 3 ? "Cancelado" 
													: "-";

				item.DescricaoSituacaoInsumo = item.SituacaoInsumo == 1 ? "Ativo" 
												: item.SituacaoInsumo == 2 ? "Inativo" 
												: item.SituacaoInsumo == 3 ? "Alterado" 
												: item.SituacaoInsumo == 4 ? "Cancelado" 
												: "-";
				if(item.TipoInsumo == "P")
				{
					if (!String.IsNullOrEmpty(item.CodigoNCM))
					{
						item.CodigoNCMFormatado = String.Format(item.CodigoNCM, "D8");
						var view = _uowSciex.QueryStackSciex.ViewInsumoPadrao.Listar(o => o.CodigoProduto == pagedFilter.CodigoProdutoSuframa 
																							&& 
																							o.CodigoNCMMercadoria == item.CodigoNCM).LastOrDefault();
						item.IdCodigoNCM = view == null ? 0 : view.CodigoProduto;
					}
				}
				else if (item.TipoInsumo != "P")
				{
					if (!String.IsNullOrEmpty(item.CodigoNCM))
					{
						item.CodigoNCMFormatado = String.Format(item.CodigoNCM, "D8");
						item.IdCodigoNCM = _uowSciex.QueryStackSciex.ViewNcm.Selecionar(o => o.CodigoNCM == item.CodigoNCMFormatado ).IdNcm;
					}
				}
				
				if (item.CodigoDetalhe > 0)
				{
					var detalhe = _uowSciex.QueryStackSciex.ViewInsumoPadrao.Selecionar(o => 
										o.CodigoProduto == pagedFilter.CodigoProdutoSuframa
										&& 
										o.CodigoNCMMercadoria == item.CodigoNCM 
										&& 
										o.CodigoDetalheMercadoria == item.CodigoDetalhe);

					item.IdCodigoDetalhe = detalhe == null ? 0 :  detalhe.IdInsumoPadrao;
				}
					
				if (item.CodigoUnidadeMedida > 0)
				{
					var obj = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == item.CodigoUnidadeMedida);
					item.DescricaoUnidadeMedida = obj.Sigla + " | " + obj.Descricao;
				}


				var verificaExiteCopia = _uowSciex.QueryStackSciex.LEInsumo.Listar(o => o.CodigoInsumo == item.CodigoInsumo
																					&& o.IdLe == item.IdLe
																					&& o.SituacaoInsumo == null);

				if (verificaExiteCopia.Count > 0)
				{
					item.ExisteCopia = true;
				}
			}

			//if (ordenacaoPosteriorCampoTipoInsumo != null)
			//{
			//	if (pagedFilter.Reverse)
			//	{
			//		insumos.Items.OrderBy(q => q.TipoInsumo).ThenBy(q => q.TipoInsumo);
			//	}
			//	else
			//	{
			//		insumos.Items.OrderBy(q => q.TipoInsumo).ThenByDescending(q => q.TipoInsumo);
			//	}
				
			//}

			return insumos;
		}

		public LEInsumoVM Salvar(LEInsumoVM vm)
		{
			if (vm == null)
				return new LEInsumoVM() { MensagemErro = "Não foi possivel incluir o Insumo!" };

			if(vm.Path == "Alterar")
			{
				if (VerificaExisteInsumo(vm))
					return new LEInsumoVM() { MensagemErro = "Insumo já cadastrado!" };

				var insumo = _uowSciex.QueryStackSciex.LEInsumo.Selecionar(o => o.IdLeInsumo == vm.IdLeInsumo);

				insumo.TipoInsumo = vm.TipoInsumo;
				insumo.CodigoNCM = vm.CodigoNCM;
				insumo.CodigoDetalhe = vm.CodigoDetalhe;
				insumo.DescricaoInsumo = vm.DescricaoInsumo;
				insumo.CodigoUnidadeMedida = vm.CodigoUnidadeMedida;
				insumo.ValorCoeficienteTecnico = vm.ValorCoeficienteTecnico;
				insumo.CodigoPartNumber = vm.CodigoPartNumber;
				insumo.DescricaoEspecTecnica = vm.DescricaoEspecTecnica;

				_uowSciex.CommandStackSciex.LEInsumo.Salvar(insumo);
				_uowSciex.CommandStackSciex.Save();

				vm.Mensagem = "Salvo com sucesso";
			}
			else if(vm.Path == "Cadastrar")
			{
				if (VerificaExisteInsumo(vm))
					return new LEInsumoVM() { MensagemErro = "Insumo já cadastrado!" };

				var ultimoInsumo = _uowSciex.QueryStackSciex.LEInsumo.Listar(o => o.CodigoProduto == vm.CodigoProduto && o.IdLe == vm.IdLe).LastOrDefault();

				if (ultimoInsumo == null)
					vm.CodigoInsumo = 1;
				else
					vm.CodigoInsumo = ultimoInsumo.CodigoInsumo + 1;

				var insumo = new LEInsumoEntity();
				insumo.SituacaoInsumo = 1; //bloq
				insumo.IdLe = vm.IdLe;
				insumo.CodigoProduto = vm.CodigoProduto;
				insumo.CodigoInsumo = vm.CodigoInsumo;
				insumo.TipoInsumo = vm.TipoInsumo;
				insumo.CodigoNCM = vm.CodigoNCM;
				insumo.CodigoDetalhe = vm.CodigoDetalhe;
				insumo.DescricaoInsumo = vm.DescricaoInsumo;
				insumo.CodigoUnidadeMedida = vm.CodigoUnidadeMedida;
				insumo.ValorCoeficienteTecnico = vm.ValorCoeficienteTecnico;
				insumo.CodigoPartNumber = vm.CodigoPartNumber;
				insumo.DescricaoEspecTecnica = vm.DescricaoEspecTecnica;

				_uowSciex.CommandStackSciex.LEInsumo.Salvar(insumo);
				_uowSciex.CommandStackSciex.Save();

				vm.Mensagem = "Salvo com sucesso";

				
			}

			return vm;

		}

		public LEInsumoVM CancelarInsumo(LEInsumoVM vm)
		{
			if (vm == null)
				return new LEInsumoVM() { MensagemErro = "Não foi possivel incluir o Insumo!" };

			var insumo = _uowSciex.QueryStackSciex.LEInsumo.Selecionar(o => o.IdLeInsumo == vm.IdLeInsumo);

			var verificaExiteCopia = _uowSciex.QueryStackSciex.LEInsumo.Listar(o => o.IdLe == insumo.IdLe && o.IdLeInsumo != insumo.IdLeInsumo && o.CodigoInsumo == insumo.CodigoInsumo && o.SituacaoInsumo == null);

			if (verificaExiteCopia.Count > 0)
			{
				vm.MensagemErro = "Não é possivel cancelar insumos com alteração em andamento!";
				return vm;
			}

			insumo.TipoInsumoAlteracao = 3;

			_uowSciex.CommandStackSciex.LEInsumo.Salvar(insumo);
			_uowSciex.CommandStackSciex.Save();

			vm.Mensagem = "Salvo com sucesso";

			return vm;

		}

		public LEInsumoVM AlterarInsumoBloq(LEInsumoVM vm)
		{
			if (vm == null)
				return new LEInsumoVM() { MensagemErro = "Não foi possivel incluir o Insumo!" };

			var insumo = _uowSciex.QueryStackSciex.LEInsumo.Selecionar(o => o.IdLeInsumo == vm.IdLeInsumo);

			if (insumo.TipoInsumo.Equals("P"))
			{
				var detalhe = _uowSciex.QueryStackSciex.ViewInsumoPadrao.Selecionar(o =>
												o.CodigoProduto == insumo.CodigoProduto
												&&
												o.CodigoNCMMercadoria == insumo.CodigoNCM
												&&
												o.CodigoDetalheMercadoria == insumo.CodigoDetalhe);

				vm.IdCodigoDetalhe = detalhe == null ? 0 : detalhe.IdInsumoPadrao; 
			}

			if (insumo.SituacaoInsumo != null)
			{
				var verificaExiteCopia = _uowSciex.QueryStackSciex.LEInsumo.Listar(o => o.CodigoInsumo == insumo.CodigoInsumo
																					&& o.IdLeInsumo != insumo.IdLeInsumo
																					&& o.IdLe == insumo.IdLe
																					&& o.SituacaoInsumo == null);

				if (verificaExiteCopia.Count > 0)
				{
					vm.MensagemErro = "Já existe cópia em alteração para este insumo.";
					return vm;
				}
			}
			else
			{
				vm.Mensagem = "Salvo com sucesso";
				return vm;
			}

			if (insumo.TipoInsumoAlteracao == 3)
			{
				insumo.TipoInsumoAlteracao = null;
				_uowSciex.CommandStackSciex.LEInsumo.Salvar(insumo);
				_uowSciex.CommandStackSciex.Save();
				_uowSciex.CommandStackSciex.DetachEntries();
			}

			//if (insumo.SituacaoInsumo == 2)
			//{
			//	insumo.SituacaoInsumo = 3;
			//	_uowSciex.CommandStackSciex.LEInsumo.Salvar(insumo);
			//	_uowSciex.CommandStackSciex.Save();
			//	_uowSciex.CommandStackSciex.DetachEntries();
			//}
			

			insumo.SituacaoInsumo = null;
			insumo.IdLeInsumo = 0;
			insumo.LEProduto = null;
			insumo.TipoInsumoAlteracao = 2;
			_uowSciex.CommandStackSciex.LEInsumo.Salvar(insumo, true);
			_uowSciex.CommandStackSciex.Save();

			vm = _uowSciex.QueryStackSciex.LEInsumo.Selecionar<LEInsumoVM>(o => o.IdLeInsumo == insumo.IdLeInsumo);


			vm.Mensagem = "Salvo com sucesso";

			return vm;

		}

		public bool VerificaExisteProduto(LEProdutoVM vm)
		{
			var obj = _uowSciex.QueryStackSciex.LEProduto.Listar(o =>
			//														o.Cnpj.Equals(vm.Cnpj)
			//														&&
																	o.CodigoProdutoSuframa.Equals(vm.CodigoProdutoSuframa)
																	&&
																	o.CodigoTipoProduto.Equals(vm.CodigoTipoProduto)
																	&&
																	o.CodigoNCM.Equals(vm.CodigoNCM)
																	&&
																	o.CodigoUnidadeMedida.Equals(vm.CodigoUnidadeMedida)
																	&&
																	o.DescricaoModelo.ToLower().Equals(vm.DescricaoModelo.ToLower())
																);
			if (obj != null && obj.Count > 0)
				return true;
			else
				return false;
		}

		public bool VerificaExisteInsumo(LEInsumoVM vm)
		{
			var codigoUnidadeMedidaInt = Convert.ToInt32(vm.CodigoUnidadeMedida);
			var obj = _uowSciex.QueryStackSciex.LEInsumo.Listar(o =>
																	o.IdLeInsumo != vm.IdLeInsumo
																	&&
																	o.CodigoNCM.Equals(vm.CodigoNCM)
																	&&
																	o.CodigoDetalhe.Equals(vm.CodigoDetalhe)
																	&&
																	o.TipoInsumo.Equals(vm.TipoInsumo)
																	&&
																	o.DescricaoInsumo.Equals(vm.DescricaoInsumo)
																	&&
																	o.DescricaoEspecTecnica.Equals(vm.DescricaoEspecTecnica)
																	&&
																	o.ValorCoeficienteTecnico.Equals(vm.ValorCoeficienteTecnico)
																	&&
																	o.CodigoUnidadeMedida == codigoUnidadeMedidaInt
																	&&
																	o.CodigoProduto == vm.CodigoProduto
																);
			if (obj != null && obj.Count > 0)
				return true;
			else
				return false;
		}

		public LEInsumoVM Selecionar(int id)
		{
			var leInsumo = _uowSciex.QueryStackSciex.LEInsumo.Selecionar<LEInsumoVM>(x => x.IdLeInsumo == id);

			if (leInsumo.listaInsumoErro != null && leInsumo.listaInsumoErro.Count > 0)
			{
				leInsumo.UltimoInsumoErro = leInsumo.listaInsumoErro.LastOrDefault();
			}

			leInsumo.DescricaoTipoInsumo = leInsumo.TipoInsumo;
			leInsumo.DescricaoTipoInsumoAlteracao = leInsumo.TipoInsumoAlteracao == 1 ? "Novo" : leInsumo.TipoInsumoAlteracao == 2 ? "Alterado" : leInsumo.TipoInsumoAlteracao == 3 ? "Cancelado" : "-";
			leInsumo.DescricaoSituacaoInsumo = leInsumo.SituacaoInsumo == 1 ? "Ativo" : leInsumo.SituacaoInsumo == 2 ? "Inativo" : leInsumo.SituacaoInsumo == 3 ? "Alterado" : leInsumo.SituacaoInsumo == 4 ? "Cancelado" : "-";
			if (leInsumo.TipoInsumo == "P")
			{
				if (!String.IsNullOrEmpty(leInsumo.CodigoNCM))
				{
					leInsumo.CodigoNCMFormatado = String.Format(leInsumo.CodigoNCM, "D8");
					var view = _uowSciex.QueryStackSciex.ViewInsumoPadrao.Selecionar(o => o.CodigoProduto == leInsumo.CodigoProduto 
																						&& o.CodigoNCMMercadoria == leInsumo.CodigoNCM);
					leInsumo.IdCodigoNCM = view == null ? 0 : view.CodigoProduto;
				}
			}
			else if (leInsumo.TipoInsumo != "P")
			{
				if (!String.IsNullOrEmpty(leInsumo.CodigoNCM))
				{
					leInsumo.CodigoNCMFormatado = String.Format(leInsumo.CodigoNCM, "D8");
					leInsumo.IdCodigoNCM = _uowSciex.QueryStackSciex.ViewNcm.Selecionar(o => o.CodigoNCM == leInsumo.CodigoNCMFormatado).IdNcm;
				}
			}

			if (leInsumo.CodigoDetalhe > 0)
			{
				var detalhe = _uowSciex.QueryStackSciex.ViewDetalheMercadoria.Selecionar(o =>
									o.CodigoProduto == leInsumo.CodigoProduto
									&&
									o.CodigoNCMMercadoria == leInsumo.CodigoNCM
									&&
									o.CodigoDetalheMercadoria == leInsumo.CodigoDetalhe);

				leInsumo.IdCodigoDetalhe = detalhe == null ? 0 : detalhe.IdDetalheMercadoria;
			}

			if (leInsumo.CodigoUnidadeMedida > 0)
			{
				var obj = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == leInsumo.CodigoUnidadeMedida);
				leInsumo.DescricaoUnidadeMedida = obj.Sigla + " | " + obj.Descricao;
			}

			//if (ordenacaoPosteriorCampoTipoInsumo != null)
			//{
			//	if (pagedFilter.Reverse)
			//	{
			//		insumos.Items.OrderBy(q => q.TipoInsumo).ThenBy(q => q.TipoInsumo);
			//	}
			//	else
			//	{
			//		insumos.Items.OrderBy(q => q.TipoInsumo).ThenByDescending(q => q.TipoInsumo);
			//	}

			//}

			return leInsumo;
		}

		public LEAnaliseInsumoVM SelecionarInsumoAtualEAnterior(int id)
		{
			LEAnaliseInsumoVM retorno = new LEAnaliseInsumoVM();

			var insumo = _uowSciex.QueryStackSciex.LEInsumo.Selecionar<LEInsumoVM>(x => x.IdLeInsumo == id);
			var insumoOriginal = _uowSciex.QueryStackSciex.LEInsumo.Listar<LEInsumoVM>(x => (x.SituacaoInsumo == (int?)EnumSituacaoInsumo.ATIVO_OU_SIM
																								||
																								x.SituacaoInsumo == (int?)EnumSituacaoInsumo.INATIVO_OU_NAO
																								||
																								x.SituacaoInsumo == (int?)EnumSituacaoInsumo.ALTERADO
																								)
																								&&
																								x.CodigoInsumo == insumo.CodigoInsumo
																								&&
																								x.IdLe == insumo.IdLe
																								).OrderByDescending(q=> q.IdLeInsumo).FirstOrDefault();

			CarregarOrganizarDadosInsumo(insumoOriginal);

			var insumoAlterado = _uowSciex.QueryStackSciex.LEInsumo.Selecionar<LEInsumoVM>(x => (x.TipoInsumoAlteracao == (int?)EnumTipoInsumoAlteracao.ALTERADO
																									||
																									x.TipoInsumoAlteracao == (int?)EnumTipoInsumoAlteracao.CANCELADO
																								)
																								&&																								
																								x.SituacaoInsumo == null
																								&&
																								x.CodigoInsumo == insumo.CodigoInsumo
																								&&
																								x.IdLe == insumo.IdLe
																								);
			CarregarOrganizarDadosInsumo(insumoAlterado);

			retorno.InsumoOriginal = insumoOriginal;
			retorno.InsumoAlterado = insumoAlterado;

			return retorno;
		}

		private void CarregarOrganizarDadosInsumo(LEInsumoVM insumoOriginal)
		{
			if (insumoOriginal.listaInsumoErro != null && insumoOriginal.listaInsumoErro.Count > 0)
			{
				insumoOriginal.UltimoInsumoErro = insumoOriginal.listaInsumoErro.LastOrDefault();
			}

			insumoOriginal.DescricaoTipoInsumo = insumoOriginal.TipoInsumo;
			insumoOriginal.DescricaoTipoInsumoAlteracao = insumoOriginal.TipoInsumoAlteracao == 1 ? "Novo" : insumoOriginal.TipoInsumoAlteracao == 2 ? "Alterado" : insumoOriginal.TipoInsumoAlteracao == 3 ? "Cancelado" : "-";
			insumoOriginal.DescricaoSituacaoInsumo = insumoOriginal.SituacaoInsumo == 1 ? "Ativo" : insumoOriginal.SituacaoInsumo == 2 ? "Inativo" : insumoOriginal.SituacaoInsumo == 3 ? "Alterado" : insumoOriginal.SituacaoInsumo == 4 ? "Cancelado" : "-";
			if (insumoOriginal.TipoInsumo == "P")
			{
				if (!String.IsNullOrEmpty(insumoOriginal.CodigoNCM))
				{
					insumoOriginal.CodigoNCMFormatado = String.Format(insumoOriginal.CodigoNCM, "D8");
					var view = _uowSciex.QueryStackSciex.ViewInsumoPadrao.Listar(o => o.CodigoProduto == insumoOriginal.CodigoProdutoSuframa
																							&&
																							o.CodigoNCMMercadoria == insumoOriginal.CodigoNCM).LastOrDefault();
					insumoOriginal.IdCodigoNCM = view == null ? 0 : view.CodigoProduto;
										
				}
			}
			else if (insumoOriginal.TipoInsumo != "P")
			{
				if (!String.IsNullOrEmpty(insumoOriginal.CodigoNCM))
				{
					insumoOriginal.CodigoNCMFormatado = String.Format(insumoOriginal.CodigoNCM, "D8");
					insumoOriginal.IdCodigoNCM = _uowSciex.QueryStackSciex.ViewNcm.Selecionar(o => o.CodigoNCM == insumoOriginal.CodigoNCMFormatado).IdNcm;
				}
			}

			if (insumoOriginal.CodigoDetalhe > 0)
			{
				var detalhe = _uowSciex.QueryStackSciex.ViewDetalheMercadoria.Selecionar(o =>
									o.CodigoProduto == insumoOriginal.CodigoProduto
									&&
									o.CodigoNCMMercadoria == insumoOriginal.CodigoNCM
									&&
									o.CodigoDetalheMercadoria == insumoOriginal.CodigoDetalhe);

				insumoOriginal.IdCodigoDetalhe = detalhe == null ? 0 : detalhe.IdDetalheMercadoria;
			}

			if (insumoOriginal.CodigoUnidadeMedida > 0)
			{
				var obj = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == insumoOriginal.CodigoUnidadeMedida);
				insumoOriginal.DescricaoUnidadeMedida = obj.Sigla + " | " + obj.Descricao;
			}

			//if (ordenacaoPosteriorCampoTipoInsumo != null)
			//{
			//	if (pagedFilter.Reverse)
			//	{
			//		insumos.Items.OrderBy(q => q.TipoInsumo).ThenBy(q => q.TipoInsumo);
			//	}
			//	else
			//	{
			//		insumos.Items.OrderBy(q => q.TipoInsumo).ThenByDescending(q => q.TipoInsumo);
			//	}

			//}
		}

		public void Deletar(long id)
		{
			var leInsumo = _uowSciex.QueryStackSciex.LEInsumo.Selecionar(s => s.IdLeInsumo == id);

			_uowSciex.CommandStackSciex.LEInsumo.Apagar(leInsumo.IdLeInsumo);

			_uowSciex.CommandStackSciex.Save();
		}

		public int DeletarLeInsumoOriginal(long id)
		{
			var insumo = _uowSciex.QueryStackSciex.LEInsumo.Selecionar(o => o.IdLeInsumo == id);

			_uowSciex.CommandStackSciex.LEInsumo.Apagar(insumo.IdLeInsumo);

			var listaInsumosComMesmaLe = _uowSciex.QueryStackSciex.LEInsumo.Listar(o => o.CodigoInsumo > insumo.CodigoInsumo
																						&& o.IdLe == insumo.IdLe);

			foreach (var registroInsumo in listaInsumosComMesmaLe)
			{
				registroInsumo.CodigoInsumo = registroInsumo.CodigoInsumo - 1;
				_uowSciex.CommandStackSciex.LEInsumo.Salvar(registroInsumo);
			}

			_uowSciex.CommandStackSciex.Save();

			return 1;
		}

	}
}