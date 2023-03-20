import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { DepartmentService } from 'src/app/services/department.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Component({
  selector: 'app-dept-list',
  templateUrl: './dept-list.component.html',
  styleUrls: ['./dept-list.component.css']
})

export class DeptListComponent {

  departments: any = [];
  departmentId: any;

  public isButtonVisible: boolean = false;
  public isEditButtonVisible: boolean = false;

  public isSubAdminButtonVisible: boolean = false;
  public isSubAdminEditButtonVisible: boolean = false;

  constructor(private router: Router, private departmentService: DepartmentService, private toastr: ToastrService, private localStorageService: LocalStorageService) {
  }

  ngOnInit(): void {
    var currentuser: any = [];
    currentuser = this.localStorageService.getLocalStorage('permission');
    let currentUser = JSON.parse(sessionStorage.getItem('role') || '{}');

    const userRole = JSON.parse(currentUser);
    this.getDeptList();
    var userData;
    var editData;

    for (var i = 0; i < userRole.length; i++) {
      if (currentuser && userRole[i] == "admin") {
        if (userData = currentuser.find((data: any) => data.value == "Delete" && data.valueType == "admin")) {
          this.isButtonVisible = true;
        }
        if (editData = currentuser.find((e: any) => e.value == "Edit" && e.valueType == "admin")) {
          this.isEditButtonVisible = true;
        }
      }
      else if (userRole[i] == "SubAdmin") {
        if (userData = currentuser.find((data: any) => data.value == "Delete" && data.valueType == "SubAdmin")) {
          this.isSubAdminButtonVisible = true;
        }
        if (editData = currentuser.find((e: any) => e.value == "Edit" && e.valueType == "SubAdmin")) {
          this.isSubAdminEditButtonVisible = true;
        }
      }
    }
  }

  getDeptList() {
    this.departmentService.getAllDepartment().subscribe((res) => {
      this.departments = res;
      setTimeout(() => {
        $('#depTable').DataTable({
          pagingType: 'full_numbers',
          pageLength: 5,
          processing: true,
          order: [[1]],
          lengthMenu: [5, 10, 25]
        });
      }, 1);
    }, error => console.error(error));
  }

  onEdit(id: number) {
    this.router.navigateByUrl('dashboard/department/addDept/' + id);
  }

  deleteDeptData(id: number) {
    var reponse = confirm("Are you sure want to delete this record?")
    if (reponse)
      return this.departmentService.deleteDept(id).subscribe((result: any) => {
        this.toastr.success("Student record deleted successfully", 'Success');
        this.ngOnInit();
      });
    return reponse;
  }
}
