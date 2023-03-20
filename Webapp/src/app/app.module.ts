import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DataTablesModule } from 'angular-datatables';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './componets/login/login.component';
import { SignupComponent } from './componets/signup/signup.component';
import { GoogleLoginProvider, SocialLoginModule, SocialAuthServiceConfig } from 'angularx-social-login';
import { LocalStorageService } from './services/local-storage.service';
import { AuthGaurdService } from './services/auth-gaurd.service';
import { DatePipe } from '@angular/common';
import { AuthService } from './services/auth.service';
import { JwtInterceptor } from './services/JwtInterceptor';
import { DeptListComponent } from './componets/dept-list/dept-list.component';
import { EmpListComponent } from './componets/emp-list/emp-list.component';
import { AddEmpComponent } from './componets/add-emp/add-emp.component';
import { AddDeptComponent } from './componets/add-dept/add-dept.component';
import { employeeModule } from './componets/emp-list/employee.module';
import { deptModule } from './componets/dept-list/dept.module';
import { PageComponent } from './componets/page/page.component';
import { MainComponent } from './componets/page/main/main.component';
import { MenuSidebarComponent } from './componets/page/menu-sidebar/menu-sidebar.component';
import { FooterComponent } from './componets/page/footer/footer.component';
import { HeaderComponent } from './componets/page/header/header.component';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { ToastrModule } from 'ngx-toastr';
import { ToastrService } from 'ngx-toastr';
import { UserListComponent } from './componets/user-list/user-list.component';
import { userModule } from './componets/user-list/user.module';
import { RoleListComponent } from './componets/role-list/role-list.component';
import { roleModule } from './componets/role-list/role.module';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,
    DeptListComponent,
    EmpListComponent,
    AddEmpComponent,
    AddDeptComponent,
    PageComponent,
    MainComponent,
    MenuSidebarComponent,
    FooterComponent,
    HeaderComponent,
    UserListComponent,
    RoleListComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    DataTablesModule,
    ReactiveFormsModule,
    HttpClientModule,
    SocialLoginModule,
    employeeModule,
    deptModule,
    NgxDropzoneModule,
    ToastrModule.forRoot({
      timeOut: 5000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    }),
    userModule,
    roleModule,
    FormsModule
  ],
  providers: [LocalStorageService, AuthGaurdService, AuthService, DatePipe, ToastrService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(
              '776259358119-gt5g7pe3i4jdthcjhad8ekvvpoovgivp.apps.googleusercontent.com', {
              scope: 'email',
              plugin_name: 'Test Project'
            }
            )
          }
        ]
      } as SocialAuthServiceConfig
    },
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
