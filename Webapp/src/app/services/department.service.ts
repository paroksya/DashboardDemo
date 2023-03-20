import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Department } from 'src/app/models/department';
import { Observable } from 'rxjs';
import { APIResponse } from 'src/app/models/apiresponse';

declare var $: any;
@Injectable({
  providedIn: 'root'
})

export class DepartmentService {
  private Url: string = "http://localhost:5286/api/Department/"
  constructor(private http: HttpClient) { }

  public getAllDepartment() {
    return this.http.get(`${this.Url}GetDepartment`)
      .pipe(map(result => {
        console.log(result);
        return result;
      }));
  }

  public addDepartment(data: Department) {
    let headers = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    };
    return this.http.post(`${this.Url}AddDepartment`, data, headers)
      .pipe(map(result => {
        return result;
      }));
  }

  public updateDepartment(data: Department) {
    let headers = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };
    return this.http.post(`${this.Url}UpdateDept`, data, headers)
      .pipe(map(result => {
        return result;
      }));
  }

  public getDeptById(id: number): Observable<APIResponse> {
    return this.http.get<APIResponse>(`${this.Url}GetDepartmentByID/` + id)
      .pipe(map(result => {
        return result;
      }));
  }

  public deleteDept(id: number) {
    return this.http.delete(`${this.Url}DeleteDepartment/` + id)
      .pipe(map(result => {
        return result;
      }));
  }
}
