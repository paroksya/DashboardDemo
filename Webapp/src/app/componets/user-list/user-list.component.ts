import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Roles } from 'src/app/models/roles';
import { Applicationuser } from 'src/app/models/applicationuser';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})

export class UserListComponent {
  users: any = [];
  userForm: any = FormGroup;
  role = new FormControl('')
  roleList: Roles[] = [];

  userIdData: any;
  userFirstName: any;
  userLastName: any;
  userIsActive: any;
  userEmail: any;
  userRole: any;
  formControls: any;

  multiData: any = [];
  removeRoles: any = [];

  userCheckedData: Applicationuser[] = [];
  rolesData: Roles[] = [];
  result: any = [];
  arrayData: any = [];
  roleNameConvert: any;
  errorMsg?: string;
  removeRoleArray: any = [];

  constructor(private auth: AuthService, private toastr: ToastrService, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.getUserList();

    this.auth.getRoleNameList().subscribe((res) => {
      res.data.forEach((obj: any) => {
      });
      this.roleList = res.data;
      this.formControls = res.data.map((control: any) => new FormControl(false));
      this.userForm = this.formBuilder.group({
        role: new FormArray(this.formControls),
      })
    });
  }

  getUserList() {
    this.auth.getAllUser().subscribe((res) => {
      this.users = res;
      setTimeout(() => {
        $('#userTable').DataTable({
          pagingType: 'full_numbers',
          pageLength: 5,
          processing: true,
          order: [[1]],
          lengthMenu: [5, 10, 25],
        });
      }, 1);
      $('#userTable tbody').on('click', 'tr', function () {
        $("#imagemodal").show();
      });
    }, error => console.error(error));
  }

  onChangePolicy(roleName: string, event: any) {
    this.errorMsg = "";
    const checked = event.target.checked;

    if (checked) {
      this.multiData.push(roleName);
    } else {
      this.removeRoles.push(roleName);
    }
  }
  onAddRoles(userId: any, firstName: string, lastName: string, email: string, isActive: boolean, roleName: string) {
    this.userIdData = userId;
    this.userFirstName = firstName;
    this.userLastName = lastName;
    this.userEmail = email;
    this.userIsActive = isActive;
    this.userRole = roleName;
    this.getUserByID(userId);
  }

  getUserByID(id: any) {
    for (var i = 0; i < this.roleList.length; i++) {
      this.roleList[i].IsChecked = false;
    }

    return this.auth.getUserByID(id).subscribe(res => {
      res.data.forEach((obj: any) => {
      });

      this.userCheckedData = res.data;

      this.result = this.userCheckedData.filter((item: any) => item.id).map((x: any) => x.roleName);

      this.roleNameConvert = JSON.parse(this.result);
      for (var i = 0; i < this.roleNameConvert.length; i++) {
        this.rolesData = this.roleList.filter(x => x.roleName == this.roleNameConvert[i]);
        if (this.rolesData != null) {
          this.rolesData[0].IsChecked = true;
        }
      }
    });
  }

  Save() {
    if (this.multiData.length > 0 || this.removeRoles.length > 0) {
      const resultConvert = JSON.parse(this.result);
      resultConvert.push(...this.multiData);

      const namesToDeleteSet = new Set(this.removeRoles)
      const newArr = resultConvert.filter((name: any) => {
        return !namesToDeleteSet.has(name);
      });
      const data = JSON.stringify(newArr);
      let nRequest = {
        id: this.userIdData,
        roleName: data,
        firstName: this.userFirstName,
        lastName: this.userLastName,
        email: this.userEmail,
        isActive: this.userIsActive
      }
      this.arrayData.push(nRequest);
      if (this.arrayData.length > 0)
        this.auth.updateUserRole(this.arrayData).subscribe(result => {
        });
    }
    this.multiData.length = 0;
    this.removeRoles.length = 0;
    this.arrayData.length = 0;
    this.removeRoleArray.length = 0;
    this.toastr.success("User Roles updated Successfully", "Success");
    $("#imagemodal").hide();
    setTimeout(() => { this.ngOnInit() }, 1000)
  }

  hide() {
    $("#imagemodal").hide();
  }
}
