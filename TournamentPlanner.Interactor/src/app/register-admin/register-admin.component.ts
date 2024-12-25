import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthService, UserInfo } from '../../Shared/auth.service';
import { LoadingService } from '../../Shared/loading.service';
import { Router } from '@angular/router';
import { AddAdminDto, AdminDto, DomainRole } from '../tp-model/TpModel';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-register-admin',
  standalone: true,
  imports: [MatCardModule, ReactiveFormsModule, MatFormField, MatInputModule, MatButtonModule, CommonModule],
  templateUrl: './register-admin.component.html',
  styleUrl: './register-admin.component.scss'
})
export class RegisterAdminComponent {

  private authService = inject(AuthService);
  private loadingService = inject(LoadingService);
  private router = inject(Router);

  public errors = signal<string[] | null>(null);

  public registerAdminForm = new FormGroup({
    name: new FormControl('', [Validators.minLength(5), Validators.required]),
    phonenumber: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  });


  public async register() {
    console.log(this.registerAdminForm);
    var addAdminDto: AddAdminDto = {
      name: this.registerAdminForm.value.name ?? '',
      phoneNumber: this.registerAdminForm.value.phonenumber ?? '',
      email: this.registerAdminForm.value.email ?? '',
      password: this.registerAdminForm.value.password ?? '',
    }

    try {
      console.log(addAdminDto);
      this.loadingService.show();
      var adminDto = await firstValueFrom(this.authService.registerAdmin(addAdminDto));
      this.setUserInfo(adminDto);
      this.router.navigate(['/tp']);
      this.loadingService.hide();
    } catch (error: any) {
      const errors = error.error?.errors ? Object.values(error.error.errors).flat() : error.error?.Error ? [error.error.Error] : [];
      this.errors.set(errors);
      this.loadingService.hide();
    }
  }
  setUserInfo(adminDto: AdminDto) {
    var userInfo: UserInfo = {
      name: adminDto.name,
      email: adminDto.email,
      role: DomainRole.Admin,
      domainUserId: adminDto.id.toString()
    }
    this.authService.setCurrentUser(userInfo);
  }

}
