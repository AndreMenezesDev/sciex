import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { INTERNET_ROUTES } from './internet.routes';

@NgModule({
	imports: [RouterModule.forChild(INTERNET_ROUTES)]
})
export class InternetModule { }
