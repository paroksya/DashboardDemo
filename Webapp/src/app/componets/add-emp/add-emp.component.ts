import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { APIResponse } from 'src/app/models/apiresponse';
import { EmployeeService } from 'src/app/services/employee.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-add-emp',
  templateUrl: './add-emp.component.html',
  styleUrls: ['./add-emp.component.css']
})

export class AddEmpComponent {
  imageToShow: any;
  submitted = false;
  editMode: boolean = false;
  Imagefiles: File[] = [];
  empForm: any = FormGroup;
  employeeID: any;
  employeeName = new FormControl('', [Validators.required, Validators.pattern('[a-zA-Z][a-zA-Z ]+')])
  emailId = new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')])
  doj = new FormControl('', [Validators.required])
  department = new FormControl('', [Validators.required])
  public deptData: any;
  deptList: any;
  newdate: any = '';
  questCount?: number;

  constructor(private router: Router, public datePipe: DatePipe, private route: ActivatedRoute, private formBuilder: FormBuilder, private empService: EmployeeService, private toastr: ToastrService) {
    this.route.params.subscribe(data => {
      if (data['id'] != null) {
        this.employeeID = data['id']
        this.editMode = true
      }
    })
  }

  ngOnInit(): void {
    this.newdate = this.datePipe.transform(this.doj.value, "yyyy-Mm-dd");

    this.getDeptNameList();
    if (this.editMode) {
      this.getEmpByID();
    }

    this.empForm = this.formBuilder.group({
      employeeID: this.employeeID,
      employeeName: this.employeeName,
      emailId: this.emailId,
      doj: this.doj.setValue(this.newdate),
      department: this.department,
      photoFileName: []
    })
  }

  get f() { return this.empForm.controls; }
  onSubmit() {
    this.submitted = true;
    if (this.Imagefiles.length < 1) {
      this.toastr.error('Please Upload atleast one image file', 'Error');
      return;
    }
    if (this.empForm.valid) {
      var fd = new FormData();
      fd.append('EmployeeName', this.employeeName.value?.toString() ?? '');
      fd.append('EmailId', this.emailId.value?.toString() ?? '');
      fd.append('DepartmentId', this.department.value?.toString() ?? '');
      fd.append('DOJ', this.datePipe.transform(this.newdate?.toString(), 'yyyy-MM-dd') ?? '');
      Array.from(this.Imagefiles).forEach(file => {
        fd.append('Files', file);
      });

      if (!this.editMode) {
        return this.empService.addEmployee(fd)
          .subscribe((result: APIResponse) => {
            if (result.success) {
              this.Imagefiles = [];
              console.log(this.Imagefiles = []);
              this.toastr.success("Employee Record Added Successfully", "Success");
              this.router.navigateByUrl('dashboard/employee');
              this.empForm.reset();
              this.submitted = false;
            }
          });
      }
      else {
        try {
          fd.append('EmployeeId', this.employeeID)
          return this.empService.updateEmployee(fd).subscribe((result: APIResponse) => {
            if (result.success) {
              this.toastr.success("Employee Record Updated Successfully", "Success");
              this.router.navigateByUrl('dashboard/employee');
              this.empForm.reset();
              this.submitted = false;
            }
          })
        }
        catch (exception) {
          console.log(exception);
        }
      }
    }
    return this.empForm.value;
  }

  onSelectImage(event: any) {
    this.Imagefiles.push(...event.addedFiles);
    if (this.Imagefiles.length == 0) {
      this.questCount = 0;
    }
  }

  onRemoveImage(event: any) {
    this.Imagefiles.splice(this.Imagefiles.indexOf(event), 1);
    if (this.Imagefiles.length == 0) {
      this.questCount = 0;
    }
  }

  getDeptNameList() {
    return this.empService.getDeptNameList().subscribe(res => {
      this.deptList = res.data;
      this.deptData = [];
      this.deptList.forEach((element: any) => {
        this.deptData.push({ id: "" + element.departmentId + "", text: element.departmentName });
      });
    });
  }

  getEmpByID() {
    return this.empService.getEmpById(this.employeeID)
      .subscribe(result => {

        if (result.success) {
          this.empForm = this.formBuilder.group({
            employeeName: this.employeeName = new FormControl(result.data['employeeName']),
            department: this.department = new FormControl(result.data['departmentId']),
            emailId: this.emailId = new FormControl(result.data['emailId']),
            doj: this.datePipe.transform(result.data['doj'], 'yyyy-MM-dd'),
            photoFileName: []
          });
          this.getBase64ImageFromUrl(environment.baseImagePath + result.data['photoFileName']).then((res: any) => {
            const arrayBase64 = this.convertDataURIToBinary(res.toString());
            const img = new File([arrayBase64], result.data['photoFileName'], { type: 'image/jpg' });
            this.Imagefiles.push(img);
          }).catch(err => console.log(err));
        }
        else {
          this.toastr.error(result.message, 'Error');
        }
      });
  }

  convertDataURIToBinary(dataURI: string) {
    var BASE64_MARKER = ';base64,';
    var base64Index = dataURI.indexOf(BASE64_MARKER) + BASE64_MARKER.length;
    var base64 = dataURI.substring(base64Index);
    var raw = window.atob(base64);
    var rawLength = raw.length;
    var array = new Uint8Array(new ArrayBuffer(rawLength));

    for (var i = 0; i < rawLength; i++) {
      array[i] = raw.charCodeAt(i);
    }
    return array;
  }

  async getBase64ImageFromUrl(imageUrl: any) {
    try {
      var res = await fetch(imageUrl);
      var blob = await res.blob();

      return new Promise((resolve, reject) => {
        var reader = new FileReader();
        reader.addEventListener("load", function () {
          resolve(reader.result);
        }, false);

        reader.onerror = () => {
          return reject(this);
        };
        reader.readAsDataURL(blob);
      })
    }
    catch (exception) {
      console.log(exception);
    }
  }
}