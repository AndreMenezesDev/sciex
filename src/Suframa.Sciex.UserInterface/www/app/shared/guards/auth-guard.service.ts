import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild } from '@angular/router';

@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild {

	active: boolean;
	
	canActivate() {
		return this.active;
	}

	canActivateChild() {
		return false;
	}

}
