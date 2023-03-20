import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Department } from 'src/app/models/department';
import { DepartmentService } from 'src/app/services/department.service';

@Component({
  selector: 'app-add-dept',
  templateUrl: './add-dept.component.html',
  styleUrls: ['./add-dept.component.css']
})

export class AddDeptComponent {
  submitted = false;
  editMode: boolean = false;
  deptForm: any = FormGroup;
  departmentId: any;
  departmentName = new FormControl('', [Validators.required])

  constructor(private router: Router, private route: ActivatedRoute, private formBuilder: FormBuilder, private deptService: DepartmentService, private toastr: ToastrService) {
    this.route.params.subscribe(data => {
      if (data['id'] != null) {
        this.departmentId = data['id']
        this.editMode = true
      }
    })
  }

  get f() { return this.deptForm?.controls; }
  ngOnInit(): void {
    this.getDeptByID();

    this.deptForm = this.formBuilder.group({
      departmentId: this.departmentId,
      departmentName: this.departmentName,
    })
  }

  onSubmit(event: any) {
    this.submitted = true;

    if (this.deptForm?.valid) {
      let userObj = new Department();
      userObj.DepartmentName = this.departmentName.value?.toString();

      if (!this.editMode) {
        return this.deptService.addDepartment(userObj)
          .subscribe((result) => {
            this.toastr.success("Department Record Added Successfully", "Success");
            this.router.navigateByUrl('dashboard/department');
            this.deptForm.reset();
            this.submitted = false;
          });
      }
      else {
        let userObj1 = new Department();
        userObj1.DepartmentId = this.departmentId;
        userObj1.DepartmentName = this.departmentName.value?.toString();

        return this.deptService.updateDepartment(userObj1)
          .subscribe((result) => {
            this.toastr.success("Department Record Updated Successfully", "Success");
            this.router.navigateByUrl('dashboard/department');
            this.deptForm.reset();
            this.submitted = false;
          });
      }
    }
    return this.deptForm.value;
  }

  getDeptByID() {
    return this.deptService.getDeptById(this.route.snapshot.params['id'])
      .subscribe(result => {
        if (result.success) {
          this.deptForm = this.formBuilder.group({
            departmentName: this.departmentName = new FormControl(result.data['departmentName'])
          })
        }
        else {
          this.toastr.error(result.message, 'Error');
        }
      });
  }
}
