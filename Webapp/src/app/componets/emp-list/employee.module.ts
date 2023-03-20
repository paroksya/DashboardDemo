import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { EmpListComponent } from '../emp-list/emp-list.component';
import { AddEmpComponent } from '../add-emp/add-emp.component';

const routes: Routes = [
  { path: '', component: EmpListComponent },
  { path: 'addEmp', component: AddEmpComponent },
  { path: 'addEmp/:id', component: AddEmpComponent }
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  entryComponents: [AddEmpComponent, EmpListComponent]
})
export class employeeModule { }
