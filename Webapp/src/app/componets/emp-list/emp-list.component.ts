import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { EmployeeService } from 'src/app/services/employee.service';
import { ToastrService } from 'ngx-toastr';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Component({
  selector: 'app-emp-list',
  templateUrl: './emp-list.component.html',
  styleUrls: ['./emp-list.component.css']
})

export class EmpListComponent {

  employees: any = [];
  employeeID: any;

  public isButtonVisible: boolean = false;
  public isEditButtonVisible: boolean = false;
  public isViewButtonVisible: boolean = false;

  constructor(private router: Router, private employeeService: EmployeeService, private toastr: ToastrService, private localStorageService: LocalStorageService) {
  }

  ngOnInit(): void {
    var currentuser: any = [];
    currentuser = this.localStorageService.getLocalStorage('permission');

    this.getEmpList();

    var userData;
    var editData;

    if (currentuser) {
      if (userData = currentuser.find((data: any) => data.value == "Delete" && data.valueType == "User")) {
        this.isButtonVisible = true;
      }
      if (editData = currentuser.find((e: any) => e.value == "Edit" && e.valueType == "User")) {
        this.isEditButtonVisible = true;
      }
    }
  }

  getEmpList() {
    this.employeeService.getAllEmployee().subscribe((res) => {
      this.employees = res;
      setTimeout(() => {
        $('#empTable').DataTable({
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
    this.router.navigateByUrl('dashboard/employee/addEmp/' + id);
  }

  deleteDeptData(id: number) {
    var reponse = confirm("Are you sure want to delete this record?")
    if (reponse)
      return this.employeeService.deleteEmp(id).subscribe((result: any) => {
        this.toastr.success("Student record deleted successfully", 'Success');
        this.ngOnInit();
      });
    return reponse;
  }
}

