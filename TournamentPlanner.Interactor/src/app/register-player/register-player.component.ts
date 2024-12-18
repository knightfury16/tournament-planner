import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-register-player',
  standalone: true,
  imports: [MatCardModule, ReactiveFormsModule, MatFormField, MatInputModule, MatButtonModule, CommonModule],
  templateUrl: './register-player.component.html',
  styleUrl: './register-player.component.scss'
})
export class RegisterPlayerComponent {

  public registerPlayerForm = new FormGroup({
    name: new FormControl('', [Validators.minLength(5), Validators.required,]), 
    age: new FormControl(),
    weight: new FormControl(),
    email: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  });


  public register() {
    console.log(this.registerPlayerForm);
  }

}
