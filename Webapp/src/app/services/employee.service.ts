import { Injectable } from '@angular/core';
import { HttpClient, } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { APIResponse } from 'src/app/models/apiresponse';

@Injectable({
  providedIn: 'root'
})

export class EmployeeService {
  private Url: string = "http://localhost:5286/api/Employee/"

  constructor(private http: HttpClient) { }

  public getAllEmployee() {
    return this.http.get(`${this.Url}GetEmployee`)
      .pipe(map(result => {
        return result;
      }));
  }

  public addEmployee(data: any) {
    return this.http.post(`${this.Url}AddEmployees`, data)
      .pipe(map(result => {
        return result;
      }));
  }

  public updateEmployee(data: any) {
    return this.http.post(`${this.Url}UpdateEmployees`, data)
      .pipe(map(result => {
        return result;
      }));
  }

  public getDeptNameList(): Observable<APIResponse> {
    return this.http.get<APIResponse>(`${this.Url}GetDeptNameList`)
      .pipe(map(result => {
        return result;
      }));
  }

  public getEmpById(id: number): Observable<APIResponse> {
    return this.http.get<APIResponse>(`${this.Url}GetEmpByID/` + id)
      .pipe(map(result => {
        return result;
      }));
  }

  public deleteEmp(id: number) {
    return this.http.delete(`${this.Url}DeleteEmployee/` + id)
      .pipe(map(result => {
        return result;
      }));
  }
}
