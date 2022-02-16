using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Linq;
using System.Text;

namespace Suframa.Sciex.BusinessLogic
{
	public class TransferirSaldoInsumoBll: ITransferirSaldoInsumoBll
	{

		private readonly IUnitOfWorkSciex _uowSciex;

		public TransferirSaldoInsumoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}

		public PRCInsumoVM PesquisarPRCInsumo(int idInsumo)
		{
			if (idInsumo == 0)
				return new PRCInsumoVM();

			var regInsumo = _uowSciex.QueryStackSciex.PRCInsumo.SelecionarGrafo(w => new PRCInsumoVM()
			{
				IdInsumo = w.IdInsumo,
				CodigoInsumo = w.CodigoInsumo,
				DescricaoInsumo = w.DescricaoInsumo,
				TipoInsumo = w.TipoInsumo,
				CodigoNCM = w.CodigoNCM,
				CodigoUnidade = w.CodigoUnidade,
				ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
				DescricaoPartNumber = w.DescricaoPartNumber,
				ValorPercentualPerda = w.ValorPercentualPerda != null ? w.ValorPercentualPerda : 0,
				DescricaoEspecificacaoTecnica = w.DescricaoEspecificacaoTecnica,
				IdPrcProduto = w.IdPrcProduto,
				ValorAdicional = w.ValorAdicional,
				QuantidadeAdicional = w.QuantidadeAdicional,
				ValorDolarSaldo = w.ValorDolarSaldo,
				QuantidadeSaldo = w.QuantidadeSaldo,
				Produto = new PRCProdutoVM()
				{
					IdProcesso = w.PrcProduto.IdProcesso,
					QuantidadeAprovado = w.PrcProduto.QuantidadeAprovado
				},
				ListaDetalheInsumos = w.ListaDetalheInsumos.Select(q => new PRCDetalheInsumoVM()
				{
					IdPrcInsumo = q.IdPrcInsumo,
					IdMoeda = q.IdMoeda,
					IdDetalheInsumo = q.IdDetalheInsumo,
					CodigoPais = q.CodigoPais,
					NumeroSequencial = q.NumeroSequencial,
					Quantidade = q.Quantidade,
					ValorDolar = q.ValorDolar,
					ValorDolarCFR = q.ValorDolarCFR,
					ValorDolarFOB = q.ValorDolarFOB,
					ValorFrete = q.ValorFrete,
					ValorUnitario = q.ValorUnitario
				}).ToList()
			},
			q => q.IdInsumo == idInsumo);

			var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == regInsumo.CodigoUnidade);
			regInsumo.DescCodigoUnidade = undMed != null ? undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao : "-";

			var qtdProduto = regInsumo?.Produto?.QuantidadeAprovado ?? 0;
			var coefTec = regInsumo?.ValorCoeficienteTecnico ?? 0;
			var percPerda = regInsumo?.ValorPercentualPerda ?? 0;

			regInsumo.QtdMaxInsumo = qtdProduto * coefTec;

			regInsumo.QtdMaxInsumo = regInsumo.QtdMaxInsumo + (regInsumo.QtdMaxInsumo * (percPerda / 100));

			return regInsumo;
		}

		public LEInsumoVM PesquisarLEInsumo(LEInsumoVM obj)
		{
			var LEInsumo = _uowSciex.QueryStackSciex.LEInsumo.SelecionarGrafo( l => new LEInsumoVM()
			{
				IdLeInsumo = l.IdLeInsumo,
				CodigoInsumo = l.CodigoInsumo,
				DescricaoInsumo = l.DescricaoInsumo,
				TipoInsumo = l.TipoInsumo,
				CodigoNCM = l.CodigoNCM,
				CodigoUnidadeMedida = l.CodigoUnidadeMedida,
				ValorCoeficienteTecnico = l.ValorCoeficienteTecnico,
				DescricaoEspecTecnica = l.DescricaoEspecTecnica,
				SituacaoInsumo = l.SituacaoInsumo,
				CodigoProduto = l.CodigoProduto,
				CodigoDetalhe = l.CodigoDetalhe,
			},
			q => q.CodigoInsumo == obj.CodigoInsumo && q.CodigoProduto == obj.CodigoProduto &&
			    (q.TipoInsumo == "P" || q.TipoInsumo == "E"));

			if (LEInsumo.CodigoUnidadeMedida > 0)
			{
				var objeto = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == LEInsumo.CodigoUnidadeMedida);
				LEInsumo.DescricaoUnidadeMedida = objeto.Sigla + " | " + objeto.Descricao;
			}

			var retPrcInsumo = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(p => p.CodigoInsumo == obj.CodigoInsumo && p.IdPrcProduto == obj.IdProduto);

			if (retPrcInsumo != null)
			{
				LEInsumo.ExisteNoProcesso = true;
			}
			else
			{
				LEInsumo.ExisteNoProcesso = false;
			}

			return LEInsumo;
		}

		public bool SalvarPrcInsumo(PRCInsumoVM InsumoVM)
		{

			if (InsumoVM.CodigoInsumo == 0)
			{
				return false;
			}

			var prcSolicAlteracaoEntity = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao
																 .Selecionar(p => p.IdProcesso == InsumoVM.Produto.IdProcesso &&
																p.Status == 1); //Em Elaboração

			#region PRCInsumo
			var prcInsumoEntity = new PRCInsumoEntity()
			{
				IdInsumo = 0,
				IdPrcProduto = InsumoVM.IdPrcProduto,
				CodigoInsumo = InsumoVM.CodigoInsumo,
				CodigoUnidade = InsumoVM.CodigoUnidade,
				TipoInsumo = InsumoVM.TipoInsumo,
				CodigoNCM = InsumoVM.CodigoNCM,
				CodigoDetalhe = InsumoVM.CodigoDetalhe,
				DescricaoInsumo = InsumoVM.DescricaoInsumo,
				DescricaoEspecificacaoTecnica = InsumoVM.DescricaoEspecificacaoTecnica,
				StatusInsumo = 2, //Em Alteração
				ValorAdicional = InsumoVM.ValorAdicional,
				QuantidadeAdicional = InsumoVM.QuantidadeAdicional,
				ValorDolarSaldo = InsumoVM.ValorDolarSaldo,
				QuantidadeSaldo = InsumoVM.QuantidadeSaldo,
				DescricaoPartNumber = InsumoVM.DescricaoPartNumber,
				ValorPercentualPerda = InsumoVM.ValorPercentualPerda,
				IdPrcSolicitacaoAlteracao = prcSolicAlteracaoEntity.Id
			};

			#region PRCDetalheInsumo

			foreach (var itemPRCDetalhe in InsumoVM.ListaDetalheInsumos)
			{
				var prcDetalheInsumoEntity = new PRCDetalheInsumoEntity()
				{
					IdDetalheInsumo = 0,
					IdPrcInsumo = prcInsumoEntity.IdInsumo,
					IdMoeda = itemPRCDetalhe.IdMoeda,
					CodigoPais = itemPRCDetalhe.CodigoPais,
					NumeroSequencial = itemPRCDetalhe.NumeroSequencial,
					Quantidade = itemPRCDetalhe.Quantidade,
					ValorDolar = itemPRCDetalhe.ValorDolar,
					ValorDolarCFR = itemPRCDetalhe.ValorDolarCFR,
					ValorDolarFOB = itemPRCDetalhe.ValorDolarFOB,
					ValorFrete = itemPRCDetalhe.ValorFrete,
					ValorUnitario = itemPRCDetalhe.ValorUnitario

				};

				

				#region PRCSolicDetalhe
				var prcSolicDetalheEntity = new PRCSolicDetalheEntity()
				{
					DescricaoDe = InsumoVM.CodigoInsumoOrigem.ToString(),
					DescricaoPara = InsumoVM.CodigoInsumoDestino.ToString(),
					IdInsumo = prcInsumoEntity.IdInsumo,
					IdDetalheInsumo = prcDetalheInsumoEntity.IdDetalheInsumo,
					IdSolicitacaoAlteracao = prcSolicAlteracaoEntity.Id,
					IdTipoSolicitacao = 2 //Transferencia de Saldo de Insumo
				};

				prcDetalheInsumoEntity.PRCSolicDetalhe.Add(prcSolicDetalheEntity);
				prcInsumoEntity.PRCSolicDetalhe.Add(prcSolicDetalheEntity);

				_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(prcSolicDetalheEntity);


				prcInsumoEntity.ListaDetalheInsumos.Add(prcDetalheInsumoEntity);

				_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(prcDetalheInsumoEntity);

				#endregion
			}

			#endregion

			_uowSciex.CommandStackSciex.PRCInsumo.Salvar(prcInsumoEntity);
			_uowSciex.CommandStackSciex.Save();
			#endregion

			return true;
		}
	}
}
