import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { parametroAnalistaVM } from '../../../view-model/ParametroAnalistaVM';


@Component({
    selector: 'app-modal-visualizar-parametrizacao',
    templateUrl: './modal-visualizar-parametrizacao.component.html',
})

export class ModalVisualizarParametrizacaoComponent {
    model: parametroAnalistaVM = new parametroAnalistaVM();
    listaStatusAnaliseVisual: any = {};
    listaStatusAnaliseListagem: any = {};
    @ViewChild('appModalVisualizarParametrizacao') appModalVisualizarParametrizacao;
    @ViewChild('appModalVisualizarParametrizacaoBackground') appModalVisualizarParametrizacaoBackground;

    public abrir(model: parametroAnalistaVM) {
		this.model = model;
        this.listaStatusAnaliseVisual = model.listaStatusAnaliseVisual;
        this.listaStatusAnaliseListagem = model.listaStatusAnaliseListagem;
        this.appModalVisualizarParametrizacao.nativeElement.style.display = 'block';
        this.appModalVisualizarParametrizacaoBackground.nativeElement.style.display = 'block';
    }

    public fechar() {
        this.appModalVisualizarParametrizacao.nativeElement.style.display = 'none';
        this.appModalVisualizarParametrizacaoBackground.nativeElement.style.display = 'none';
    }

}
