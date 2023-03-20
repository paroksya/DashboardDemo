import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthGaurdService } from '../services/auth-gaurd.service';

@Injectable({
  providedIn: 'root'
})

export class AuthenticationGuard implements CanActivate {

  constructor(private authGuard: AuthGaurdService, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) {
    if (this.authGuard.isLoggedIn()) {
      setTimeout(() => {
        this.router.navigate(['/login']);
        sessionStorage.clear();
      }, 400000);
      return true;
    }
    alert('you are not logged in');
    this.router.navigate(['login']);
    return false;
  }
}
