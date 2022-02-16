import { Injectable } from '@angular/core';

@Injectable()
export class LoadingService {
	public show() {
		document.getElementById('loading').style.display = 'block';
	}

	public hide() {
		document.getElementById('loading').style.display = 'none';
	}
}
