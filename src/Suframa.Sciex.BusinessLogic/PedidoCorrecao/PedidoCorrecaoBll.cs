using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Json;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic
{
	public class PedidoCorrecaoBll : IPedidoCorrecaoBll
	{
		private readonly IUnitOfWork _uow;
		private readonly IUsuarioLogado _usuarioLogado;

		public PedidoCorrecaoBll(
			IUnitOfWork uow,
			IUsuarioLogado usuarioLogado)
		{
			_uow = uow;
			_usuarioLogado = usuarioLogado;
		}

		private List<ResumoVM> FormatarListaResumo(List<PedidoCorrecaoEntity> listaPedidoCorrecao)
		{
			var listaResumoVM = new List<ResumoVM>();
			var campoSistema = _uow.QueryStack.CampoSistema.Listar();
			var descricaoDropDown = _uow.QueryStack.DicionarioDropDown.Listar();

			foreach (var pedidoCorrecaoEntity in listaPedidoCorrecao)
			{
				var resumoVM = Mapper.Map<ResumoVM>(pedidoCorrecaoEntity);

				var diff = Json.ObterDiferencas(pedidoCorrecaoEntity.CampoDe, pedidoCorrecaoEntity.CampoPara);

				if (diff.Count.Equals(0))
				{
					resumoVM.ValorDe = null;
					resumoVM.ValorPara = null;

					listaResumoVM.Add(resumoVM);
				}

				foreach (var item in diff)
				{
					var diffResumo = new ResumoVM { Acao = resumoVM.Acao, Secao = resumoVM.Secao, IdCampoSistema = resumoVM.IdCampoSistema, Justificativa = resumoVM.Justificativa, Status = resumoVM.Status, DataSolicitacao = resumoVM.DataSolicitacao, IdPedidoCorrecao = resumoVM.IdPedidoCorrecao, Campo = resumoVM.Campo, CampoObjeto = resumoVM.CampoObjeto, CampoTela = resumoVM.CampoTela, Descricao = resumoVM.Descricao };

					var campoTela = String.Join(",", item.Split(';')[2]).ToUpper();

					var campo = campoSistema.Find(s => s.CampoTela.ToUpper().Contains(campoTela) && s.Secao.Contains(resumoVM.Secao));

					if (campo != null)
					{
						diffResumo.Descricao = campo.DescricaoCampo;
					}

					var dicionario = descricaoDropDown.FindAll(s => s.Secao.Contains(resumoVM.Secao) && s.Campo.Contains(campoTela));

					if (dicionario.Count != 0)
					{
						int number;
						var valorDe = Int32.Parse(item.Split(';')[0]);
						var valorPara = Int32.TryParse(item.Split(';')[1], out number);

						var listDe = dicionario.FindAll(x => x.Valor.Equals(valorDe));
						var listPara = dicionario.FindAll(y => y.Valor.Equals(number));

						diffResumo.ValorDe = listDe[0].Descricao;
						diffResumo.ValorPara = listPara.Count >= 1 ? listPara[0].Descricao : null;
					}
					else
					{
						diffResumo.ValorDe = String.Join(",", item.Split(';')[0]);
						diffResumo.ValorPara = String.Join(",", item.Split(';')[1]);
					}

					listaResumoVM.Add(diffResumo);
				}
			}

			return listaResumoVM;
		}

		private void SalvarCorrecaoAlteracaoAdministradores(PedidoCorrecaoEntity pedidoCorrecao)
		{
			RegrasSalvar(new PedidoCorrecaoEntity
			{
				DataSolicitacao = DateTime.Now,
				Status = (int)EnumStatusPedidoCorrecao.Aberto,
				IdUsuarioSistema = _usuarioLogado.Usuario.IdUsuarioInterno,
				IdCampoSistema = 79,
				IdProtocolo = pedidoCorrecao.IdProtocolo,
				Justificativa = "Revisar os documentos",
				Acao = (int)EnumAcaoCampoSistema.Alterar,
				CampoDe = "[{'null':'null' }]",
				CampoPara = "[{'null':'null' }]"
			}, true);
		}

		private void SalvarCorrecaoAlteracaoNaturezaJuridica(PedidoCorrecaoEntity pedidoCorrecao)
		{
			var idNaturezaJuridica = Convert.ToInt32(Json.ExtrairPrimeiroValorPrimeiroCampo(pedidoCorrecao.CampoPara));

			var naturezaJuridica = _uow.QueryStack.NaturezaJuridica.Selecionar(x => x.IdNaturezaJuridica == idNaturezaJuridica);

			RegrasSalvar(new PedidoCorrecaoEntity
			{
				DataSolicitacao = DateTime.Now,
				Status = (int)EnumStatusPedidoCorrecao.Aberto,
				IdUsuarioSistema = _usuarioLogado.Usuario.IdUsuarioInterno,
				IdCampoSistema = 35,
				IdProtocolo = pedidoCorrecao.IdProtocolo,
				Justificativa = "Verificar qualificação dos administradores",
				Acao = (int)EnumAcaoCampoSistema.Adicionar,
				CampoDe = "[{'null':'null' }]",
				CampoPara = "[{'null':'null' }]"
			}, true);

			RegrasSalvar(new PedidoCorrecaoEntity
			{
				DataSolicitacao = DateTime.Now,
				Status = (int)EnumStatusPedidoCorrecao.Aberto,
				IdUsuarioSistema = _usuarioLogado.Usuario.IdUsuarioInterno,
				IdCampoSistema = 79,
				IdProtocolo = pedidoCorrecao.IdProtocolo,
				Justificativa = "Revisar os documentos",
				Acao = (int)EnumAcaoCampoSistema.Alterar,
				CampoDe = "[{'null':'null' }]",
				CampoPara = "[{'null':'null' }]"
			}, true);

			if (naturezaJuridica == null || !naturezaJuridica.StatusQuadroSocial) { return; }

			var protocolo = _uow.QueryStack.Protocolo.Selecionar(x => x.IdProtocolo == pedidoCorrecao.IdProtocolo);

			if (protocolo?.Requerimento?.PessoaJuridica?.PessoaJuridicaSocio?.Count > 0) { return; }

			RegrasSalvar(new PedidoCorrecaoEntity
			{
				DataSolicitacao = DateTime.Now,
				Status = (int)EnumStatusPedidoCorrecao.Aberto,
				IdUsuarioSistema = _usuarioLogado.Usuario.IdUsuarioInterno,
				IdCampoSistema = 40,
				IdProtocolo = pedidoCorrecao.IdProtocolo,
				Justificativa = "Incluir ao menos um sócio",
				Acao = (int)EnumAcaoCampoSistema.Adicionar,
				CampoDe = "[{'null':'null' }]",
				CampoPara = "[{'null':'null' }]"
			}, true);
		}

		private void SalvarCorrecaoAlteracaoSocio(PedidoCorrecaoEntity pedidoCorrecao)
		{
			RegrasSalvar(new PedidoCorrecaoEntity
			{
				DataSolicitacao = DateTime.Now,
				Status = (int)EnumStatusPedidoCorrecao.Aberto,
				IdUsuarioSistema = _usuarioLogado.Usuario.IdUsuarioInterno,
				IdCampoSistema = 79,
				IdProtocolo = pedidoCorrecao.IdProtocolo,
				Justificativa = "Revisar os documentos",
				Acao = (int)EnumAcaoCampoSistema.Alterar,
				CampoDe = "[{'null':'null' }]",
				CampoPara = "[{'null':'null' }]"
			}, true);
		}

		public void Apagar(int? id)
		{
			if (!id.HasValue) { return; }

			_uow.CommandStack.PedidoCorrecao.Apagar(id.Value);
			_uow.CommandStack.Save();
		}

		public IEnumerable<ResumoVM> ListarItensAtualizar(ProtocoloVM protocoloVM)
		{
			var listaPedidoCorrecao = _uow.QueryStack.PedidoCorrecao.Listar(x =>
				(x.IdProtocolo == protocoloVM.IdProtocolo) &&
				(!x.IdUsuarioSistema.HasValue)
			);

			if (listaPedidoCorrecao == null || !listaPedidoCorrecao.Any()) { return null; }

			return FormatarListaResumo(listaPedidoCorrecao);
		}

		public IEnumerable<ResumoVM> ListarItensCorrecao(ProtocoloVM protocoloVM)
		{
			var listaPedidoCorrecao = _uow.QueryStack.PedidoCorrecao.Listar(x =>
				(x.IdProtocolo == protocoloVM.IdProtocolo) &&
				(x.IdUsuarioSistema.HasValue)
			);

			if (listaPedidoCorrecao == null || !listaPedidoCorrecao.Any()) { return null; }

			return FormatarListaResumo(listaPedidoCorrecao);
		}

		public IEnumerable<ResumoVM> ListarItensCorrigidos(ProtocoloVM protocoloVM)
		{
			//Seleciona SOMENTE os itens que TEM data de correção
			var listaPedidoCorrecao = _uow.QueryStack.PedidoCorrecao.Listar(x =>
				(x.IdProtocolo == protocoloVM.IdProtocolo) &&
				(x.DataCorrecao.HasValue) &&
				(x.IdUsuarioSistema.HasValue)
			);

			if (listaPedidoCorrecao == null || !listaPedidoCorrecao.Any()) { return null; }

			return FormatarListaResumo(listaPedidoCorrecao);
		}

		public IEnumerable<ResumoVM> ListarItensCorrigir(ProtocoloVM protocoloVM)
		{
			//Seleciona SOMENTE os itens que NÃO TEM data de correção
			var listaPedidoCorrecao = _uow.QueryStack.PedidoCorrecao.Listar(x =>
				(x.IdProtocolo == protocoloVM.IdProtocolo) &&
				(!x.DataCorrecao.HasValue) &&
				(x.IdUsuarioSistema.HasValue)
			);

			if (listaPedidoCorrecao == null || !listaPedidoCorrecao.Any()) { return null; }

			return FormatarListaResumo(listaPedidoCorrecao);
		}

		public void RegrasSalvar(PedidoCorrecaoEntity pedidoCorrecao, bool IsChamadaInterna = false)
		{
			var pedidoExistente = _uow.QueryStack.PedidoCorrecao.Selecionar(x =>
			x.IdCampoSistema == pedidoCorrecao.IdCampoSistema &&
			x.IdProtocolo == pedidoCorrecao.IdProtocolo &&
			x.IdTabela == pedidoCorrecao.IdTabela &&
			x.Status == (int)EnumStatusPedidoCorrecao.Aberto);

			if (pedidoCorrecao.IdCampoSistema == 5)
			{
				SalvarCorrecaoAlteracaoNaturezaJuridica(pedidoCorrecao);
			}

			if (pedidoCorrecao.IdCampoSistema == 35 && !IsChamadaInterna)
			{
				SalvarCorrecaoAlteracaoAdministradores(pedidoCorrecao);
			}

			if (pedidoCorrecao.IdCampoSistema == 40 && !IsChamadaInterna)
			{
				SalvarCorrecaoAlteracaoSocio(pedidoCorrecao);
			}

			if (pedidoExistente != null)
			{
				pedidoExistente.CampoPara = pedidoCorrecao.CampoPara;
				pedidoExistente.DataSolicitacao = pedidoCorrecao.DataSolicitacao;
				pedidoExistente.IdUsuarioSistema = pedidoCorrecao.IdUsuarioSistema;
				pedidoExistente.Justificativa = pedidoCorrecao.Justificativa;
				pedidoExistente.Acao = pedidoCorrecao.Acao;

				_uow.CommandStack.PedidoCorrecao.Salvar(pedidoExistente);

				return;
			}

			_uow.CommandStack.PedidoCorrecao.Salvar(pedidoCorrecao);
		}

		public void Salvar(PedidoCorrecaoVM[] pedidoCorrecaoVM)
		{
			foreach (var pedido in pedidoCorrecaoVM)
			{
				var pedidoCorrecao = Mapper.Map<PedidoCorrecaoEntity>(pedido);
				pedidoCorrecao.DataSolicitacao = DateTime.Now;
				pedidoCorrecao.Status = (int)EnumStatusPedidoCorrecao.Aberto;
				pedidoCorrecao.IdUsuarioSistema = _usuarioLogado.Usuario.IdUsuarioInterno;

				RegrasSalvar(pedidoCorrecao);
			}

			_uow.CommandStack.Save();
		}

		public PedidoCorrecaoVM Selecionar(int id)
		{
			var pedido = _uow.QueryStack.PedidoCorrecao.Selecionar(x => x.IdPedidoCorrecao == id);

			return Mapper.Map<PedidoCorrecaoVM>(pedido);
		}
	}
}