import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { arquivoVM } from '../../view-model/ArquivoVM';
import { ApplicationService } from './application.service';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ArquivoService {
	servico = 'Arquivo';

	constructor(
		private applicationService: ApplicationService,
		private http: HttpClient,
	) { }

	upload(file) {
		const model: arquivoVM = new arquivoVM();
		model.nome = file.name;
		model.tamanho = file.size;
		model.tipo = file.type;

		const formData: FormData = new FormData();
		formData.append('File', file, file.name);
		formData.append('Model', JSON.stringify(model));

		return Observable.create(observer => {
			this.applicationService.put(this.servico, formData)
				.subscribe(result => {
					observer.next(result);
					observer.complete();
				});
		});
	}
}
