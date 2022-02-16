import { Injectable } from '@angular/core';

@Injectable()
export class ObjectService {
	public clone(obj: any): any {
		return JSON.parse(JSON.stringify(obj));
	}
}
