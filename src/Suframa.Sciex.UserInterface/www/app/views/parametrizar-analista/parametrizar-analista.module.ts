import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ParametrizarAnalistaComponent } from './parametrizar-analista.component';
import { ParametrizarAnalistaGridComponent } from './grid/grid.component';
import { ModalVisualizarParametrizacaoComponent } from './modal/modal-visualizar-parametrizacao.component';
import { ModalBloquearAnalistaComponent } from './modal/modal-bloquear-analista.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
        SharedModule,
    ],
    declarations: [
        ParametrizarAnalistaComponent,
        ParametrizarAnalistaGridComponent,
        ModalVisualizarParametrizacaoComponent,        
        ModalBloquearAnalistaComponent,
        ParametrizarAnalistaGridComponent,
        ModalVisualizarParametrizacaoComponent
    ],
    providers: []
})
export class ParametrizarAnalistaModule { }
