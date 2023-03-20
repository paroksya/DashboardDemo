import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Policy } from 'src/app/models/policy';
import { RoleBasedPolicy } from 'src/app/models/role-based-policy';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.css']
})

export class RoleListComponent {
  rolesForm: any = FormGroup;
  roles: any = [];

  roleId: any;
  roleIdData: any;
  policyList: Policy[] = [];
  policy = new FormControl('')
  policyCheckedData: RoleBasedPolicy[] = [];

  formControls: any;
  errorMsg?: string;
  result: any = [];

  policyData: Policy[] = [];
  multiData: any = [];
  removePolicy: any = [];
  arrayData: any = [];

  constructor(private auth: AuthService, private toastr: ToastrService, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.getUserList();
    this.auth.getPolicyNameList().subscribe(res => {
      res.data.forEach((obj: any) => {
      });
      this.policyList = res.data;
      this.formControls = res.data.map((control: any) => new FormControl(false));
      this.rolesForm = this.formBuilder.group({
        policy: new FormArray(this.formControls),
      })
    });
  }

  getUserList() {
    this.auth.getRoleNameList().subscribe((res) => {
      this.roles = res.data;
      setTimeout(() => {
        $('#rolesTable').DataTable({
          pagingType: 'full_numbers',
          pageLength: 5,
          processing: true,
          order: [[1]],
          lengthMenu: [5, 10, 25],
        });
      }, 1);
      $('#rolesTable tbody').on('click', 'tr', function () {
        $("#imagemodal").show();
      });
    }, error => console.error(error));
  }

  onChangePolicy(policyId: number, event: any) {
    this.errorMsg = "";
    const checked = event.target.checked;

    if (checked) {
      this.multiData.push(policyId);
    } else {
      this.removePolicy.push(policyId);
    }
  }

  hide() {
    $("#imagemodal").hide();
  }

  Save() {
    for (var i = 0; i < this.multiData.length; i++) {
      let nRequest = {
        policyID: this.multiData[i],
        roleId: this.roleIdData,
        isChecked: true
      }
      this.arrayData.push(nRequest);
    }

    if (this.arrayData.length > 0)
      this.auth.addRoleBasedPolicy(this.arrayData).subscribe(result => {
      });
    this.auth.deleteRolePolicy(this.removePolicy, this.roleIdData).subscribe(result => {
    });
    this.multiData.length = 0;
    this.removePolicy.length = 0;
    this.arrayData.length = 0;
    this.toastr.success("Policy updated Successfully", "Success");
    $("#imagemodal").hide();
    setTimeout(() => { this.ngOnInit() }, 1000)
  }

  onAddRoles(roleId: any) {
    this.roleIdData = roleId;
    this.getRolePolicesByID(roleId);
  }

  getRolePolicesByID(id: number) {
    for (var i = 0; i < this.policyList.length; i++) {
      this.policyList[i].IsChecked = false;
    }

    return this.auth.getRolesBasedPoliciesByID(id).subscribe(res => {
      res.data.forEach((obj: any) => {
      });
      this.policyCheckedData = res.data;

      this.result = this.policyCheckedData.filter((item: any) => item.isChecked).map((x: any) => x.policyID);

      for (var i = 0; i < this.result.length; i++) {
        this.policyData = this.policyList.filter(x => x.policyId == this.result[i]);
        if (this.policyData != null) {
          this.policyData[0].IsChecked = true;
        }
      }
    });
  }
}