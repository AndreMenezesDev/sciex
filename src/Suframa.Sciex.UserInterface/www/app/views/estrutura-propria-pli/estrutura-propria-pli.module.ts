import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { EstruturaPropriaPLIComponent } from './estrutura-propria-pli.component';
import { ModalConsultarProtocoloEnvioComponent } from './modal/modal-estrutura-propria-pli.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
		SharedModule
    ],
    declarations: [
		EstruturaPropriaPLIComponent,
		ModalConsultarProtocoloEnvioComponent
	],
})
export class EstruturaPropriaPLIModule { }
