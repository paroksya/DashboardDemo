import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RoleListComponent } from '../role-list/role-list.component';

const routes: Routes = [
  { path: '', component: RoleListComponent }
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  entryComponents: [RoleListComponent]
})
export class roleModule { }
