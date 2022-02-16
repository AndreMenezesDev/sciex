import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ApplicationService } from "../../../shared/services/application.service";
import { MessagesService } from "../../../shared/services/messages.service";
import { ModalService } from "../../../shared/services/modal.service";
import { ManterTipoEmabalagemComponent } from "../manter-tipo-embalagem.component"

@Component({
	selector: 'app-manter-tipo-embalagem-grid',
	templateUrl: './grid.component.html'
})

export class ManterTipoEmbalagemGridComponent {

	servico = "TipoEmbalagem";

    constructor(
		private ManterTipoEmabalagemComponent : ManterTipoEmabalagemComponent,
        private applicationService: ApplicationService,
        private modal: ModalService,
        private msg: MessagesService,       
    ) { 

	}
    
    @Input() lista: any[];
	@Input() total: number;
	@Input() size: number;
	@Input() sorted: string;
	@Input() page: number;	
	@Input() exportarListagem: boolean;
	@Input() parametros: any = {};

	@Output() onChangeSort: EventEmitter<any> = new EventEmitter();
	@Output() onChangeSize: EventEmitter<any> = new EventEmitter();
	@Output() onChangePage: EventEmitter<any> = new EventEmitter();
    @Output() onChange: EventEmitter<any> = new EventEmitter();

	changeSize($event) {
		this.onChangeSize.emit(+$event);
	}

	changeSort($event) {
		this.sorted = $event.field;
        this.onChangeSort.emit($event);
        this.changePage(this.page);
	}

	changePage($event) {
		this.page = $event;
		this.onChangePage.emit($event);
    }

	confirmarAtivarStatus(item) {
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, 'Ativar Status', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.ativarStatus(item);
				}
			});
	}


	confirmarInativarStatus(item) {
		item.inativar = 1;
		this.modal.confirmacao(this.msg.CONFIRMAR_OPERACAO, 'Inativar Status', '')
			.subscribe(isConfirmado => {
				if (isConfirmado) {
					this.ativarStatus(item);
				}
			});
	}

	ativarStatus(item) {
		if (item.status == 0)
		{
			item.status = 1;
		} else
		{
			item.status = 0;
		}

		this.applicationService.put(this.servico, item).subscribe(result => {
			if (result != null) {
				this.modal.resposta(this.msg.OPERACAO_REALIZADA_COM_SUCESSO, "Sucesso", "");
				this.ManterTipoEmabalagemComponent.listar();
			}
		});
	}
}  