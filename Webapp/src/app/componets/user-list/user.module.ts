import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { UserListComponent } from '../user-list/user-list.component';

const routes: Routes = [
  { path: '', component: UserListComponent }
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  entryComponents: [UserListComponent]
})
export class userModule { }
