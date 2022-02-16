import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Injectable, NgModule } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Router, RouterModule } from '@angular/router';

import { GeneratorService } from '../services/generator.service';
import { LoadingService } from '../services/loading.service';
import { ModalService } from '../services/modal.service';
import { environment } from '../../../environments/environment';
import { AppComponent } from '../../app.component';

@Injectable()
export class HttpsRequestInterceptor implements HttpInterceptor {
	baseUrl: string;
	urlPath: string;
	constructor(		
		private generator: GeneratorService,
		private loadingService: LoadingService,
		private modal: ModalService,
		private router: Router,
		private routerModule: RouterModule,
	) {
		this.baseUrl = window.location.href;
	
	}

	urlExcessao() : string[] {
		var url = ['/manter-ncm-amazonia-ocidental', '/manter-controle-importacao',
			this.router.url];
		return url;
	}

	intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		
		const timeoutLoading = setTimeout(x => { this.loadingService.show(); }, 1000);
		
		const dupReq = req.clone({ headers: req.headers.set('FrontGuid', this.generator.guid()) });
		
		return next.handle(dupReq).do(
			success => {
				
				this.loadingService.show();

			},
            error => {
				if (error) {
					const message = error.error || error.statusText;					
					if (error.status == 302) {
						this.modal.alerta(error.error.message).subscribe(result => {
							if (result) {
								//this.router.navigate([error.error.url]);
								//window.location.reload();
							}
						});
					}
					
					if (error.status == 400 || error.status == 401) {
						this.modal.alerta(message);
					}
					else if (error.status == 409) {
						this.modal.alerta(message, '', '')
							.subscribe(isConfirmado => {
								if (isConfirmado) {

									if (!this.urlExcessao().includes(this.router.url)) {
										window.history.back();							
									}
										
								}
							});
					}
					else if (error.status == 502) {
						this.modal.alerta(message, '', '')
							.subscribe(isConfirmado => {
								if (isConfirmado) {
									window.location.reload();
								}
							});
					}
					else if (!environment.production && (error.status == 500 || error.status == 0)) {
                        this.modal.alerta('Sem conexão com o servidor. Tente novamente.', 'Error - 500')
                            .subscribe(isConfirmado => {                                
                                console.error(message);
                                //window.location.reload();
                            });                        
                       
						
					}
				}

				clearTimeout(timeoutLoading);
				this.loadingService.hide();
			},
			() => {
				clearTimeout(timeoutLoading);
				this.loadingService.hide();
			}
		);
	}
}

@NgModule({
	providers: [
		GeneratorService,
		{ provide: HTTP_INTERCEPTORS, useClass: HttpsRequestInterceptor, multi: true }
	]
})
export class InterceptorsModule { }
