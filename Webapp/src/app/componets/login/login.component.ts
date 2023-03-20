import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { GoogleLoginProvider, SocialAuthService } from 'angularx-social-login';
import { ToastrService } from 'ngx-toastr';
import { Socialusers } from 'src/app/models/socialusers';
import { TokenModel } from 'src/app/models/token-model';
import { AuthService } from 'src/app/services/auth.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent {
  response: any;
  socialusers = new Socialusers();

  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";

  loginForm: any = FormGroup;
  username = new FormControl('', [Validators.required]);
  password = new FormControl('', [Validators.required]);

  constructor(private router: Router, private authService: SocialAuthService, private fb: FormBuilder, private auth: AuthService, private localStorageService: LocalStorageService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: this.username,
      password: this.password
    })
  }

  hideShowPass() {
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type = "text" : this.type = "password";
  }

  onLogin() {
    if (this.loginForm.valid) {
      const user: any = {
        UserName: this.username.value, Password: this.password.value
      };
      this.auth.login(user).subscribe((result) => {
        if (result != null && result.success) {
          var model: TokenModel;
          model = result.data;

          this.localStorageService.setLocalStorage('token', model.token);
          this.localStorageService.setLocalStorage('role', model.user['roleName']);
          this.localStorageService.setLocalStorage('userId', model.user['id']);
          this.localStorageService.setLocalStorage('permission', model.permission);
          this.localStorageService.setLocalStorage('userName', model.user['userName']);

          this.loginForm.reset();

          const userRole = JSON.parse(model.user['roleName']);

          for (var i = 0; i < userRole.length; i++) {
            if (userRole[i] == "admin" || userRole[i] == "SubAdmin") {
              this.router.navigate(['dashboard']);
            }
            else if (userRole[i] == "User") {
              this.router.navigate(['dashboard']);
            }
          }
        }
        else {
          this.toastr.error(result.message);
        }
      })
    }
  }

  private validateAllFormFields(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      if (control instanceof FormControl) {
        control.markAsDirty({ onlySelf: true });
      } else if (control instanceof FormGroup) {
        this.validateAllFormFields(control)
      }
    })
  }

  public socialSignIn(socialProvider: string) {
    let socialPlatformProvider: any;
    if (socialProvider === 'google') {
      socialPlatformProvider = GoogleLoginProvider.PROVIDER_ID;
    }
    this.authService.signIn(socialPlatformProvider).then(socialusers => {
      this.Savesresponse(socialusers);
    });
  }

  Savesresponse(socialusers: Socialusers) {
    this.auth.Savesresponse(socialusers).subscribe((res) => {
      if (res != null && res.success) {
        var model: TokenModel;
        model = res.data;

        this.localStorageService.setLocalStorage('token', model.token);
        this.localStorageService.setLocalStorage('provider', model.user['provider']);
        this.localStorageService.setLocalStorage('permission', model.permission);
        this.localStorageService.setLocalStorage('role', model.user['roleName']);
        this.localStorageService.setLocalStorage('providerId', model.user['id']);
        this.localStorageService.setLocalStorage('userId', model.user['id']);

        this.socialusers = model;

        this.response = this.socialusers;

        const userRole = JSON.parse(model.user['roleName']);

        for (var i = 0; i < userRole.length; i++) {
          if (userRole[i] == "admin" || userRole[i] == "SubAdmin") {
            this.router.navigate(['dashboard']);
          }
          else if (userRole[i] == "User") {
            this.router.navigate(['dashboard']);
          }
        }
      }
    })
  }
}
