import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { INTRANET_EXTERNO_ROUTES } from './externo.routes';

@NgModule({
	imports: [RouterModule.forChild(INTRANET_EXTERNO_ROUTES)]
})
export class IntranetExternoModule { }
