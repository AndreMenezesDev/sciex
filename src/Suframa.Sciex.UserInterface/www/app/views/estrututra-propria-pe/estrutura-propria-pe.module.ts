import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { EstruturaPropriaPEComponent } from './estrutura-propria-pe.component';
import { ModalInformacoesPEComponent } from './modal/modal-estrutura-propria-pe.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
		SharedModule
    ],
    declarations: [
		EstruturaPropriaPEComponent,
		ModalInformacoesPEComponent
	],
})
export class EstruturaPropriaPEModule { }
