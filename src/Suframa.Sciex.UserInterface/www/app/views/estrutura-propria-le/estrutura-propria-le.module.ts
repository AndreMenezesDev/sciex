import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { EstruturaPropriaLEComponent } from './estrutura-propria-le.component';
import { ModalAjudaEnviarLEComponent } from './modal/modal-estrutura-propria-le.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
		SharedModule
    ],
    declarations: [
        EstruturaPropriaLEComponent,
        ModalAjudaEnviarLEComponent
	],
})
export class EstruturaPropriaLEModule { }
