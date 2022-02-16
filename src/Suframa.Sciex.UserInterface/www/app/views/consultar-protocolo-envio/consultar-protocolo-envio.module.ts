import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { ManterConsultarProtocoloEnvioComponent } from './consultar-protocolo-envio.component';
import { ManterConsultarProtocoloEnvioPliFormularioComponent } from './formulario/consultar-protocolo-envio-pli.component';
import { ManterConsultarProtocoloEnvioGridComponent } from './grid/grid.component';
import { ModalConsultarProtocoloEnvioComponent } from './modal/modal-consultar-protocolo-envio-informacao.component';
import { ManterConsultarProtocoloEnvioPliGridComponent } from './grid/grid-envio-pli.component';
import { ManterConsultarProtocoloEnvioPliErrosFormularioComponent } from './formulario/consultar-protocolo-envio-pli-erros.component';
import { ManterConsultarProtocoloEnvioPliErrosGridComponent } from './grid/grid-envio-pli-erros.component';
import { ManterConsultarProtocoloEnvioLeFormularioComponent } from './formulario/consultar-protocolo-envio-le.component';
import { ManterConsultarProtocoloEnvioLeGridComponent } from './grid/grid-envio-le.component';
import { ManterConsultarProtocoloEnvioLeErrosFormularioComponent } from './formulario/consultar-protocolo-envio-le-erros.component';
import { ManterConsultarProtocoloEnvioLeErrosGridComponent } from './grid/grid-envio-le-erros.component';
import { ManterConsultarProtocoloEnvioPlanoErrosFormularioComponent } from './formulario/consultar-protocolo-envio-plano-erros.component';
import { ManterConsultarProtocoloEnvioPlanoFormularioComponent } from './formulario/consultar-protocolo-envio-plano.component';
import { ManterConsultarProtocoloEnvioPlanoGridComponent } from './grid/grid-envio-plano.component';
import { ManterConsultarProtocoloEnvioPlanoErrosGridComponent } from './grid/grid-envio-plano-erros.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
		SharedModule,
    ],
    declarations: [
		ManterConsultarProtocoloEnvioComponent,      
		ManterConsultarProtocoloEnvioGridComponent,
		ManterConsultarProtocoloEnvioPliFormularioComponent,
		ModalConsultarProtocoloEnvioComponent,
		ManterConsultarProtocoloEnvioPliGridComponent,
		ManterConsultarProtocoloEnvioPliErrosFormularioComponent,
		ManterConsultarProtocoloEnvioPliErrosGridComponent,
		ManterConsultarProtocoloEnvioLeFormularioComponent,
		ManterConsultarProtocoloEnvioLeGridComponent,
		ManterConsultarProtocoloEnvioLeErrosFormularioComponent,
		ManterConsultarProtocoloEnvioLeErrosGridComponent,
		ManterConsultarProtocoloEnvioPlanoFormularioComponent,
		ManterConsultarProtocoloEnvioPlanoErrosFormularioComponent,
		ManterConsultarProtocoloEnvioPlanoGridComponent,
		ManterConsultarProtocoloEnvioPlanoErrosGridComponent,

	],
})
export class ManterConsultarProtocoloEnvioModule { }
