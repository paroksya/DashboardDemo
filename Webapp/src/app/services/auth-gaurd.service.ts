import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthGaurdService {

  constructor() { }

  isLoggedIn() {
    return !!sessionStorage.getItem('token');
  }
}
