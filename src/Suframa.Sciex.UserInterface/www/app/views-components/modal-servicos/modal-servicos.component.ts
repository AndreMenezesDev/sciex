import { ApplicationService } from '../../shared/services/application.service';
import { Component, ViewChild, EventEmitter, Output } from '@angular/core';
import { MessagesService } from '../../shared/services/messages.service';
import { servicoVM } from '../../view-model/ServicoVM';
import { ToastrService } from 'toastr-ng2';

@Component({
	selector: 'app-modal-servicos',
	templateUrl: './modal-servicos.component.html'
})
export class ModalServicosComponent {
	servicos: any[] = new Array<any>();

	@Output() onConfirmar: EventEmitter<any> = new EventEmitter();

	@ViewChild('appModalServicos') appModalServicos;
	@ViewChild('appModalServicosBackground') appModalServicosBackground;

	constructor(private applicationService: ApplicationService) { }

	public abrir(parametroAnalistaServico) {
		this.servicos = new Array<any>();
		this.load(parametroAnalistaServico);
		this.appModalServicos.nativeElement.style.display = 'block';
		this.appModalServicosBackground.nativeElement.style.display = 'block';
	}

	public fechar() {
		this.appModalServicos.nativeElement.style.display = 'none';
		this.appModalServicosBackground.nativeElement.style.display = 'none';
	}

	public confirmar() {
		if (this.servicos) {
			this.onConfirmar.emit(this.servicos.filter(function (x) { return x.isSelecionado; }));
		} else {
			this.onConfirmar.emit(new Array<any>());
		}

		this.fechar();
	}

	load(parametroAnalistaServico: any[]) {
		this.applicationService.get<Array<any>>('Servico').subscribe(result => {
			if (parametroAnalistaServico) {
				for (let i = 0; i < result.length; i++) {
					const index = parametroAnalistaServico.map((e) => { return e.idServico; }).indexOf(result[i].idServico);

					if (index > -1) {
						result[i].isSelecionado = true;
						result[i].idParametroAnalistaServico = parametroAnalistaServico[index].idParametroAnalistaServico;
					}
				}
			}

			this.servicos = result;
		});
	}
}
