import { Component, OnInit, ViewChild } from "@angular/core";
import { ApplicationService } from "../../../shared/services/application.service";
import { MessagesService } from "../../../shared/services/messages.service";
import { ModalService } from "../../../shared/services/modal.service";
import { PRCInsumoVM } from "../../../view-model/PRCInsumoVM";

@Component({
	selector: 'app-modal-transferencia-insumo',
	templateUrl: './modal-transferencia-insumo.component.html',
})

export class ModalTransferenciaInsumoComponent implements OnInit {

	servico = "TransferirSaldoInsumo";
	servicoLEI ="ConsultarLEInsumo";
	codigoProdutoExportacao: any;
	idProduto: any;

	insumoOrigem : PRCInsumoVM = new PRCInsumoVM();
	insumoDestino: any = {};
	parametros: any = {};
	desabilitado: Boolean = false;

	@ViewChild('appModalModalTransferenciaInsumo') appModalModalTransferenciaInsumo;
	@ViewChild('appModalModalTransferenciaInsumoBackground') appModalModalTransferenciaInsumoBackground;
	@ViewChild('insumo') insumo;

	constructor(
		private modal: ModalService,
		private msg: MessagesService,
		private applicationService: ApplicationService,
	) {

	}

	ngOnInit() {

	}

	public abrir(itemSelecionado){
		
		this.appModalModalTransferenciaInsumo.nativeElement.style.display = 'block';
		this.appModalModalTransferenciaInsumoBackground.nativeElement.style.display = 'block';
		this.codigoProdutoExportacao = itemSelecionado.produto.codigoProdutoExportacao;
		this.parametros.codigoProdutoExportacao = this.codigoProdutoExportacao;
		this.pesquisarInsumoOrigem(itemSelecionado.idInsumo);
	}

	pesquisarInsumoOrigem(idInsumo){

		this.applicationService.get(this.servico, idInsumo).subscribe((result: any) => {
			this.insumoOrigem = result;
			this.idProduto = this.insumoOrigem.idPrcProduto;
		});
	}

	pesquisarInsumoDestino(event) {
        const charCode = (event.which) ? event.which : event.keyCode;

		let pcodigoInsumo = this.insumo.model.split(" | ")[0].replace(" ", "");

		if (charCode == 1 && pcodigoInsumo != "") {
			
			let obj: Object = {
				idProduto: this.idProduto,
				codigoInsumo:pcodigoInsumo ,
				codigoProduto: this.codigoProdutoExportacao
			};

			this.applicationService.get(this.servicoLEI, obj).subscribe((result: any) => {
				if(result){
					this.insumoDestino = result;
				}else{
					this.modal.alerta('Código Insumo não encontrado','Insumo');
					this.limpar();
				}
			});
		}
    }

	fechar() {
		this.limpar();
		this.appModalModalTransferenciaInsumo.nativeElement.style.display = 'none';
		this.appModalModalTransferenciaInsumoBackground.nativeElement.style.display = 'none';
	}

	salvar(){

		let codigoInsumo = this.insumo.model.split(" | ")[0].replace(" ", "");

		if(codigoInsumo == undefined || codigoInsumo == null || codigoInsumo == ''){
			this.modal.alerta(this.msg.CAMPO_NAO_INFORMADO + ': Código Insumo ','Insumo');
		}else{

			if(this.insumoDestino.existeNoProcesso){
				this.modal.alerta('Insumo já está incluso no Processo','Insumo');
				this.insumo.onClear(true);
			}else{

				this.insumoDestino.idPrcProduto = this.idProduto;
				this.insumoDestino.codigoUnidade = this.insumoDestino.codigoUnidadeMedida;
				this.insumoDestino.descricaoEspecificacaoTecnica = this.insumoDestino.descricaoEspecTecnica;
				this.insumoDestino.valorAdicional = this.insumoOrigem.valorDolarAdicional,
				this.insumoDestino.quantidadeAdicional =  this.insumoOrigem.quantidadeSaldo,
				this.insumoDestino.valorDolarSaldo = this.insumoOrigem.valorDolarSaldo,
				this.insumoDestino.quantidadeSaldo = this.insumoOrigem.quantidadeSaldo
				this.insumoDestino.descricaoPartNumber = this.parametros.descricaoPartNumber;
				this.insumoDestino.valorPercentualPerda = this.parametros.valorPercentualPerda;
				this.insumoDestino.codigoInsumoOrigem = this.insumoOrigem.codigoInsumo;
				this.insumoDestino.codigoInsumoDestino = codigoInsumo;
				this.insumoDestino.listaDetalheInsumos = this.insumoOrigem.listaDetalheInsumos;
				this.insumoDestino.produto = this.insumoOrigem.produto;

				this.applicationService.post(this.servico, this.insumoDestino).subscribe((result: boolean) => {
					if(result){
						this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Insumo", "");
					}else{
						this.modal.alerta(this.msg.NAO_FOI_POSSIVEL_CONCLUIR_OPERACAO,'Insumo');
					}
				});		
			}
		}
	}

	limpar(){
		this.insumoOrigem = new PRCInsumoVM();
		this.insumo.onClear(true);
		this.insumoDestino = {};
		this.parametros = {};
	}
}