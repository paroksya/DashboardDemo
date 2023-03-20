import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DeptListComponent } from '../dept-list/dept-list.component';
import { AddDeptComponent } from '../add-dept/add-dept.component';

const routes: Routes = [
  { path: '', component: DeptListComponent },
  { path: 'addDept', component: AddDeptComponent },
  { path: 'addDept/:id', component: AddDeptComponent }
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  entryComponents: [AddDeptComponent, DeptListComponent]
})
export class deptModule { }
