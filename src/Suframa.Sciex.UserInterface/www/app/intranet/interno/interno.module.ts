import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { INTRANET_INTERNO_ROUTES } from './interno.routes';

@NgModule({
	imports: [RouterModule.forChild(INTRANET_INTERNO_ROUTES)]
})
export class IntranetInternoModule { }
