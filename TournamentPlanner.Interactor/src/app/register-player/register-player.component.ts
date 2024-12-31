import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AddPlayerDto, DomainRole, PlayerDto } from '../tp-model/TpModel';
import { AuthService, UserInfo } from '../../Shared/auth.service';
import { LoadingService } from '../../Shared/loading.service';
import { firstValueFrom } from 'rxjs';
import { Router, RouterLink } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-register-player',
  standalone: true,
  imports: [MatCardModule, ReactiveFormsModule, MatFormField, MatInputModule, MatButtonModule, CommonModule, MatIconModule, RouterLink],
  templateUrl: './register-player.component.html',
  styleUrl: './register-player.component.scss'
})
export class RegisterPlayerComponent {

  private authService = inject(AuthService);
  private loadingService = inject(LoadingService);
  private router = inject(Router);
  public hidePassword = true;

  public errors = signal<string[] | null>(null);

  public registerPlayerForm = new FormGroup({
    name: new FormControl('', [Validators.minLength(5), Validators.required,]),
    age: new FormControl(),
    weight: new FormControl(),
    email: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  });


  public async register() {
    console.log(this.registerPlayerForm);
    var addPlayerDto: AddPlayerDto = {
      name: this.registerPlayerForm.value.name ?? '',
      email: this.registerPlayerForm.value.email ?? '',
      password: this.registerPlayerForm.value.password ?? '',
      age: this.registerPlayerForm.value.age ?? 0,
      weight: this.registerPlayerForm.value.weight ?? 0
    }

    try {
      console.log(addPlayerDto);
      this.loadingService.show();
      var playerDto = await firstValueFrom(this.authService.registerPlayer(addPlayerDto));
      this.setUserInfo(playerDto);
      this.router.navigate(['/tp']);
      this.loadingService.hide();
    } catch (error: any) {
      const errors = error.error?.errors ? Object.values(error.error.errors).flat() : error.error?.Error ? [error.error.Error] : [];
      this.errors.set(errors);
      this.loadingService.hide();
    }
  }
  setUserInfo(playerDto: PlayerDto) {
    var userInfo: UserInfo = {
      name: playerDto.name,
      email: playerDto.email,
      role: DomainRole.Player,
      domainUserId: playerDto.id.toString()
    }
    this.authService.setCurrentUser(userInfo);
  }

}
