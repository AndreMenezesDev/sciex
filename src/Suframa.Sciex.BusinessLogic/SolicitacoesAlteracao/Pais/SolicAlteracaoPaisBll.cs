using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Suframa.Sciex.DataAccess;

namespace Suframa.Sciex.BusinessLogic
{
	public class SolicAlteracaoPaisBll : ISolicAlteracaoPaisBll
	{
		private int emAlteracao = 2, ativo = 1, entregue = 3;
		IQuantidadeCoeficienteBll _quantidadeCoeficienteBll;
		private readonly IUnitOfWorkSciex _uowSciex;

		public SolicAlteracaoPaisBll(IQuantidadeCoeficienteBll quantidadeCoeficienteBll, IUnitOfWorkSciex uowSciex)
		{
			_quantidadeCoeficienteBll = quantidadeCoeficienteBll;
			_uowSciex = uowSciex;
		}

		public SolicitacoesAlteracaoVM Buscar(SolicitacoesAlteracaoVM objeto)
		{
			var retornoMetodo = new SolicitacoesAlteracaoVM();

			#region Buscar Dado Duplicado

			retornoMetodo.PRCInsumoPara = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar<PRCInsumoTableColunsVM>(o => o.CodigoInsumo == objeto.PRCInsumoDE.CodigoInsumo
																												   && o.IdPrcProduto == objeto.PRCInsumoDE.IdPrcProduto
																												   && o.StatusInsumo == emAlteracao);
			if (retornoMetodo.PRCInsumoPara != null)
			{
				retornoMetodo.flagExisteRegistroDuplicado = true;
				var solicDetalhe = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Selecionar(o => o.IdInsumo == retornoMetodo.PRCInsumoPara.IdInsumo && o.IdTipoSolicitacao == 3); //PAÍS 
				if (solicDetalhe != null) //verificar se o usuario ja cadastrou alguma alteração desse tipo para o INSUMO.
				{					
					retornoMetodo.PRCDetalheInsumoPara = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar<PRCDetalheInsumoTableColunsVM>(o => o.IdDetalheInsumo == solicDetalhe.IdDetalheInsumo);
				}
			}
			else
			{
				retornoMetodo.flagExisteRegistroDuplicado = false;
			}

			#endregion

			#region Buscar Dado Original

			retornoMetodo.PRCInsumoDE = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar<PRCInsumoTableColunsVM>(o => o.CodigoInsumo == objeto.PRCInsumoDE.CodigoInsumo
																												 && o.IdPrcProduto == objeto.PRCInsumoDE.IdPrcProduto
																												 && o.StatusInsumo == ativo);    //ativo		

			retornoMetodo.PRCDetalheInsumoDE = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar<PRCDetalheInsumoTableColunsVM>(o => o.IdPrcInsumo == retornoMetodo.PRCInsumoDE.IdInsumo);
			#endregion

			retornoMetodo.QuantidadeCoefTecnicoPara = new QuantidadeCoefTecnicoVM();

			return retornoMetodo;
		}
	

		public int Salvar(SolicitacoesAlteracaoVM objeto)
		{
			var insumoEmAlteracao = new PRCInsumoEntity();

			if (objeto.flagExisteRegistroDuplicado)
			{
				insumoEmAlteracao = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.CodigoInsumo == objeto.PRCInsumoDE.CodigoInsumo
																				     && o.IdPrcProduto == objeto.PRCInsumoDE.IdPrcProduto 
																					 && o.StatusInsumo == emAlteracao);
			}

			if (!objeto.flagExisteRegistroDuplicado)
			{

				var PRCInsumoEntity = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.IdInsumo == objeto.PRCInsumoDE.IdInsumo);

				var emElaboracao = 1;
				var prcSolicAlteracaoEntity = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.IdProcesso == objeto.IdProcesso && o.Status == emElaboracao);

				if (prcSolicAlteracaoEntity == null)
					return (int)EnumStatusRetornoRequisicao.NAO_EXISTE_SOLCITACAO_ALTERACAO_CADASTRADA;

				#region SCIEX_PRC_INSUMO

				PRCInsumoEntity.StatusInsumo = emAlteracao;
				PRCInsumoEntity.CodigoInsumo = objeto.PRCInsumoDE.CodigoInsumo;
				PRCInsumoEntity.IdPrcProduto = objeto.PRCInsumoDE.IdPrcProduto;
				PRCInsumoEntity.IdPrcSolicitacaoAlteracao = prcSolicAlteracaoEntity.Id;

				#region SCIEX_PRC_DETALHE_INSUMO
				foreach (var _PRCDetalheInsumoEntity in PRCInsumoEntity.ListaDetalheInsumos)
				{					
					_PRCDetalheInsumoEntity.CodigoPais = objeto.codigoPaisPara;

					#region SCIEX_PRC_SOLIC_DETALHE

					if (objeto.PRCDetalheInsumoDE.NumeroSequencial == _PRCDetalheInsumoEntity.NumeroSequencial)
					{
						string codigoPaisDe = objeto.codigoPaisDe.ToString("D3");
						string codigoPaisPara = objeto.codigoPaisPara.ToString("D3");
						var PaisDE = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPaisDe);
						var PaisPARA = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPaisPara);
						var prcSolicDetalheEntity = new PRCSolicDetalheEntity()
						{
							DescricaoDe = PaisDE.Descricao,
							DescricaoPara = PaisPARA.Descricao,
							IdSolicitacaoAlteracao = prcSolicAlteracaoEntity.Id,
							IdTipoSolicitacao = 3 //país							
						};
						_PRCDetalheInsumoEntity.PRCSolicDetalhe.Add(prcSolicDetalheEntity);
						PRCInsumoEntity.PRCSolicDetalhe.Add(prcSolicDetalheEntity);
						_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(prcSolicDetalheEntity);
					}

					#endregion

					PRCInsumoEntity.ListaDetalheInsumos.Add(_PRCDetalheInsumoEntity);

					_PRCDetalheInsumoEntity.IdDetalheInsumo = 0;
					_PRCDetalheInsumoEntity.Moeda = null;

					_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(_PRCDetalheInsumoEntity);
				}
				#endregion

				PRCInsumoEntity.IdInsumo = 0;
				PRCInsumoEntity.StatusInsumoNovo = 0; //NAO É UM NOVO REGISTRO.
				_uowSciex.CommandStackSciex.PRCInsumo.Salvar(PRCInsumoEntity);
				_uowSciex.CommandStackSciex.Save();

				#endregion

			}
			else
			{
				var PRCInsumoEntity = insumoEmAlteracao;

				var emElaboracao = 1;
				var prcSolicAlteracaoEntity = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.IdProcesso == objeto.IdProcesso && o.Status == emElaboracao);

				if (prcSolicAlteracaoEntity == null)
					return (int)EnumStatusRetornoRequisicao.NAO_EXISTE_SOLCITACAO_ALTERACAO_CADASTRADA;

				#region SCIEX_PRC_DETALHE_INSUMO

				foreach (var _PRCDetalheInsumoEntity in PRCInsumoEntity.ListaDetalheInsumos)
				{
					_PRCDetalheInsumoEntity.CodigoPais = objeto.codigoPaisPara;

					#region SCIEX_PRC_SOLIC_DETALHE

					if (objeto.PRCDetalheInsumoDE.NumeroSequencial == _PRCDetalheInsumoEntity.NumeroSequencial)
					{
						string codigoPaisDe = objeto.codigoPaisDe.ToString("D3");
						string codigoPaisPara = objeto.codigoPaisPara.ToString("D3");
						var PaisDE = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPaisDe);
						var PaisPARA = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPaisPara);
						var prcSolicDetalheEntity = new PRCSolicDetalheEntity()
						{
							DescricaoDe = PaisDE.Descricao,
							DescricaoPara = PaisPARA.Descricao,
							IdSolicitacaoAlteracao = prcSolicAlteracaoEntity.Id,
							IdTipoSolicitacao = 3, //país
							IdDetalheInsumo = _PRCDetalheInsumoEntity.IdDetalheInsumo,
							IdInsumo = PRCInsumoEntity.IdInsumo
						};
						//_PRCDetalheInsumoEntity.PRCSolicDetalhe.Add(prcSolicDetalheEntity);
						//PRCInsumoEntity.PRCSolicDetalhe.Add(prcSolicDetalheEntity);
						_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(prcSolicDetalheEntity);
					}

					#endregion

					_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(_PRCDetalheInsumoEntity);
				}

				_uowSciex.CommandStackSciex.Save();

				#endregion

			}

			return (int) EnumStatusRetornoRequisicao.SUCESSO;
		}

		enum EnumStatusRetornoRequisicao
		{
			SUCESSO = 1,
			NAO_EXISTE_SOLCITACAO_ALTERACAO_CADASTRADA = 3
		}

	}
}
