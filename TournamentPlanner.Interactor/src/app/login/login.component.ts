import { CommonModule } from '@angular/common';
import { Component, inject, Inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterModule } from '@angular/router';
import { AuthService, UserInfo } from '../../Shared/auth.service';
import { LoginDto } from '../tp-model/TpModel';
import { LoadingService } from '../../Shared/loading.service';
import {MatInputModule} from '@angular/material/input';
import { HttpErrorResponse } from '@angular/common/http';

export type LoginErrorType = {
  Error: string
};

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ MatButtonModule,RouterModule, CommonModule,
    MatFormFieldModule,MatInputModule, FormsModule, ReactiveFormsModule,MatCardModule,MatIconModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  hidePassword = true;
  public loginError = signal<string | null>(null);
  private laodingService = inject(LoadingService);

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    // Any initialization logic can go here
  }

  login(): void {
    if (this.loginForm.valid) {
      // Handle login request here
      var loginDto: LoginDto = {
        email: this.loginForm.value.email,
        password: this.loginForm.value.password
      }
      console.log(loginDto);

      this.laodingService.show();
      this.authService.login(loginDto).subscribe(
        (response: UserInfo) => {
          // Handle successful login here
          this.authService.setCurrentUser(response);
          console.log(this.authService.getCurrentUser());
          this.laodingService.hide();
          // Redirect to home page
          this.router.navigate(['/tp']);
        },
        (error: HttpErrorResponse) => {
          // Handle login error here
          this.laodingService.hide();
          this.loginError.set(error.error.Error)
        }
      )


    }
  }
}
