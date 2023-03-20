import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './componets/login/login.component';
import { SignupComponent } from './componets/signup/signup.component';
import { AuthService } from './services/auth.service';
import { AuthenticationGuard } from './services/authentication.guard';
import { EmpListComponent } from './componets/emp-list/emp-list.component';
import { DeptListComponent } from './componets/dept-list/dept-list.component';
import { PageComponent } from './componets/page/page.component';
import { MainComponent } from './componets/page/main/main.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'page', component: PageComponent },
  {
    path: 'dashboard', component: MainComponent, canActivate: [AuthenticationGuard],
    children: [
      {
        path: 'user', loadChildren: () => import('./componets/user-list/user.module').then(m => m.userModule), canActivate: [AuthenticationGuard]
      },
      {
        path: 'roles', loadChildren: () => import('./componets/role-list/role.module').then(m => m.roleModule), canActivate: [AuthenticationGuard]
      },
      {
        path: 'department', loadChildren: () => import('./componets/dept-list/dept.module').then(m => m.deptModule), canActivate: [AuthenticationGuard]
      },
      {
        path: 'employee', loadChildren: () => import('./componets/emp-list/employee.module').then(m => m.employeeModule), canActivate: [AuthenticationGuard]
      },
    ]
  },
  { path: 'deptList', component: DeptListComponent },
  { path: 'empList', component: EmpListComponent },
  {
    path: "",
    redirectTo: '/login',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [AuthService, AuthenticationGuard]
})
export class AppRoutingModule { }
