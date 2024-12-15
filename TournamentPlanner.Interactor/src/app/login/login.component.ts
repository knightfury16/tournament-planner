import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterModule } from '@angular/router';
import { AuthService, UserInfo } from '../../Shared/auth.service';
import { LoginDto } from '../tp-model/TpModel';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MatToolbarModule, MatButtonModule, MatIconModule, RouterModule, CommonModule,
    MatFormFieldModule, FormsModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    // Any initialization logic can go here
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      // Handle login request here
      var loginDto: LoginDto = {
        email: this.loginForm.value.email,
        password: this.loginForm.value.password
      }
      console.log(loginDto);

      this.authService.login(loginDto).subscribe(
        (response: UserInfo) => {
          // Handle successful login here
          this.authService.currentUser.set(response);
          console.log(this.authService.currentUser());
          // Redirect to home page
          this.router.navigate(['/tp']);
        },
        (error) => {
          // Handle login error here
          console.log(error);
        }
      )


    }
  }
}
