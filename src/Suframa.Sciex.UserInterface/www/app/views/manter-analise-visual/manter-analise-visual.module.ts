import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterAnaliseVisualComponent } from './manter-analise-visual.component';
import { ManterAnaliseVisualGridComponent } from './grid/grid.component';
import { ManterAnaliseVisualFormularioComponent } from './formulario/formulario.component';
import { ModalAnaliseVisualComponent } from './modal/modal-analise-visual.component';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
		SharedModule,
    ],
    declarations: [
		ManterAnaliseVisualComponent,
    ManterAnaliseVisualGridComponent,
    ManterAnaliseVisualFormularioComponent,
    ModalAnaliseVisualComponent

	],
})
export class ManterAnaliseVisualModule { }
