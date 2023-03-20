import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpEvent, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';

@Injectable()

export class InterceptorService implements HttpInterceptor {

  constructor(private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
      .pipe(
        retry(1),
        catchError((error: any) => {
          let errorMessage = '';
          console.log(error)
          console.log(error.error)
          if (error instanceof HttpErrorResponse) {
            if (error.status == 0 && error.url?.match(/api/)) {
              localStorage.removeItem('token')
              this.router.navigate(['/login']);
            }
            else if (error.status == 401 && error.url?.match(/api/)) {
              localStorage.removeItem('token')
              this.router.navigate(['/login']);
            }
          }
          return throwError(errorMessage);
        })
      )
  }
}

