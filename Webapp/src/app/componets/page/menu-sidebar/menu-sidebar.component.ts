import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthGaurdService } from 'src/app/services/auth-gaurd.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Component({
  selector: 'app-menu-sidebar',
  templateUrl: './menu-sidebar.component.html',
  styleUrls: ['./menu-sidebar.component.css']
})

export class MenuSidebarComponent {

  constructor(private router: Router, private authGuard: AuthGaurdService, private localStorageService: LocalStorageService, private toastr: ToastrService) { }

  public isForAdmin: any;
  public isForUser: any;

  public isUserButtonVisible: boolean = false;
  public isUserEditButtonVisible: boolean = false;

  public isAdminButtonVisible: boolean = false;
  public isAdminEditButtonVisible: boolean = false;

  public isSuperAdminButtonVisible: boolean = false;
  public isSuperAdminEditButtonVisible: boolean = false;

  ngOnInit(): void {
    this.isUserType()
    this.hideShowPermission()
  }

  isUserType() {
    if (this.authGuard.isLoggedIn()) {
      let currentUser = JSON.parse(sessionStorage.getItem('role') || '{}');
      const userRole = JSON.parse(currentUser);
      for (var i = 0; i < userRole.length; i++) {
        if (userRole[i] == "admin" || userRole[i] == "SubAdmin") {
          this.isForAdmin = this.router.navigate(['dashboard']);
        }
        else if (userRole[i] == "User") {
          this.isForUser = this.router.navigate(['dashboard']);
        }
        return true;
      }
    }
    return false;
  }

  hideShowPermission() {
    var currentuser: any = [];
    currentuser = this.localStorageService.getLocalStorage('permission');

    var userProvider = this.localStorageService.getLocalStorage('provider');
    let currentUser = JSON.parse(sessionStorage.getItem('role') || '{}');

    const userRole = JSON.parse(currentUser);

    var viewUserData;
    var editUserData;
    var viewAdminData;
    var editAdminData;
    var viewSuperAdmin;
    var editSuperAdmin;

    for (var i = 0; i < userRole.length; i++) {
      if (currentuser && userRole[i] == "User" || userProvider == "GOOGLE") {
        if (viewUserData = currentuser.find((data: any) => data.value == "View" && data.valueType == "User")) {
          this.isUserButtonVisible = true;
          if (this.router.url === '/dashboard/employee/addEmp' && !this.isUserEditButtonVisible) {
            this.toastr.error('You have not access permission to create Employee');
          }
        }
        if (editUserData = currentuser.find((e: any) => e.value == "Create" && e.valueType == "User")) {
          this.isUserEditButtonVisible = true;
          if (this.router.url === '/dashboard/employee' && !this.isUserButtonVisible) {
            this.toastr.error('You have not access permission to View Employee data');
          }
        }
      }
      else if (currentuser && userRole[i] == "admin") {
        if (viewAdminData = currentuser.find((e: any) => e.value == "View" && e.valueType == "admin")) {
          this.isAdminButtonVisible = true;
          if (this.router.url === '/dashboard/department/addDept' && !this.isAdminEditButtonVisible) {
            this.toastr.error('You have not access permission to Create Department');
          }
        }
        if (editAdminData = currentuser.find((e: any) => e.value == "Create" && e.valueType == "admin")) {
          this.isAdminEditButtonVisible = true;
          if (this.router.url === '/dashboard/department' && !this.isAdminButtonVisible) {
            this.toastr.error('You have not access permission to View Department data');
          }
        }
      }
      else if (currentuser && userRole[i] == "SubAdmin") {
        if (viewSuperAdmin = currentuser.find((e: any) => e.value == "View" && e.valueType == "SubAdmin")) {
          this.isSuperAdminButtonVisible = true;
          if (this.router.url === '/dashboard/department/addDept' && !this.isSuperAdminEditButtonVisible) {
            this.toastr.error('You have not access permission to Create Department');
          }
        }
        if (editSuperAdmin = currentuser.find((e: any) => e.value == "Create" && e.valueType == "SubAdmin")) {
          this.isSuperAdminEditButtonVisible = true;
          if (this.router.url === '/dashboard/department' && !this.isSuperAdminButtonVisible) {
            this.toastr.error('You have not access permission to View Department data');
          }
        }
      }
    }
  }
}

