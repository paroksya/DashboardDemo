import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginUserViewModel } from '../models/login-user-view-model';
import { map, Observable } from 'rxjs';
import { APIResponse } from '../models/apiresponse';
import { Socialusers } from '../models/socialusers';
import { Router } from '@angular/router';
import { LocalStorageService } from './local-storage.service';
import { RoleBasedPolicy } from '../models/role-based-policy';
import { Applicationuser } from '../models/applicationuser';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private basrUrl: string = "http://localhost:5286/api/Authenticate/"
  private Url: string = "http://localhost:5286/api/RolePolicy/"

  constructor(private http: HttpClient, private router: Router, private localStorageService: LocalStorageService) { }

  public signUp(userObj: any) {
    return this.http.post<any>(`${this.basrUrl}RegisterAdmin`, userObj);
  }

  public login(model?: LoginUserViewModel): Observable<APIResponse> {
    return this.http.post<APIResponse>(`${this.basrUrl}Login`, model)
      .pipe(map((result: any) => {
        return result;
      }));
  }

  public Savesresponse(responce: Socialusers): Observable<APIResponse> {
    return this.http.post<APIResponse>(`${this.basrUrl}SocialRegister`, responce)
      .pipe(map((result: any) => {
        return result;
      }));
  }

  public logout() {
    this.localStorageService.clearAllStorage();
    this.localStorageService.removeStorage(['token']);
    this.router.navigateByUrl('/login');
  }

  public getAllUser(): Observable<APIResponse> {
    return this.http.get<APIResponse>(`${this.Url}GetAllUsers`)
      .pipe(map(result => {
        return result;
      }));
  }

  public getPolicyNameList(): Observable<APIResponse> {
    return this.http.get<APIResponse>(`${this.Url}GetPolicyNameList`)
      .pipe(map(result => {
        return result;
      }));
  }

  public addRoleBasedPolicy(rolePolicy: RoleBasedPolicy) {
    return this.http.post(`${this.Url}AddRoleBasedpolicy`, rolePolicy)
      .pipe(map((result: any) => {
        return result;
      }));
  }

  public getRoleNameList(): Observable<APIResponse> {
    return this.http.get<APIResponse>(`${this.Url}GetRoleNameList`)
      .pipe(map(result => {
        return result;
      }));
  }

  public addUserRole(userRole: Applicationuser) {
    return this.http.post(`${this.Url}AddUserRole`, userRole)
      .pipe(map((result: any) => {
        return result;
      }));
  }

  public deleteRolePolicy(policies: number[], roleId: number): Observable<APIResponse> {
    return this.http.post<APIResponse>(`${this.Url}DeleteMultipleRoleBasedPolicies`, policies, {
      params: {
        roleId: roleId
      }
    })
      .pipe(result => {
        return result;
      });
  }

  public getRolesBasedPoliciesByID(id: number): Observable<APIResponse> {
    return this.http.get<APIResponse>(`${this.Url}GetRolePolicyByID/` + id)
      .pipe(result => {
        return result;

      });
  }

  public updateUserRole(userRole: Applicationuser) {
    return this.http.post(`${this.Url}UpdateUserRole`, userRole)
      .pipe(map((result: any) => {
        return result;
      }));
  }

  public getUserByID(id: number): Observable<APIResponse> {
    return this.http.get<APIResponse>(`${this.Url}GetUserByID/` + id)
      .pipe(result => {
        return result;

      });
  }

  public getAllRolesBasedPolicies(): Observable<APIResponse> {
    return this.http.get<APIResponse>(`${this.Url}GetRolesBasedPolicies`)
      .pipe(map(result => {
        return result;
      }))
  }
}
