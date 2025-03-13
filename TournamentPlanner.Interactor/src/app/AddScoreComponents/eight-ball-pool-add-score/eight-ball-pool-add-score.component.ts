import { Component, signal } from '@angular/core';
import { FormControl, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { BaseAddScoreComponent } from '../base-add-score/base-add-score.component';
import { EightBallPoolScoreType } from '../../../GameTypeSuportedScore/EightBallPoolScore';

@Component({
  selector: 'app-eight-ball-pool-add-score',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatGridListModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './eight-ball-pool-add-score.component.html',
  styleUrls: ['./eight-ball-pool-add-score.component.scss']
})
export class EightBallPoolAddScoreComponent extends BaseAddScoreComponent {
  public scoreError = signal<string | undefined>(undefined);
  public scoreForm: FormGroup;
  public readonly DefaultEightBallPoolRackValue = 5;

  constructor() {
    super();
    this.scoreForm = new FormGroup({
      raceTo: new FormControl(this.DefaultEightBallPoolRackValue, [Validators.required, Validators.min(3), Validators.max(15)]),
      player1Racks: new FormControl(0, [
        Validators.required,
        Validators.min(0),
        this.racksValidator()
      ]),
      player2Racks: new FormControl(0, [
        Validators.required,
        Validators.min(0),
        this.racksValidator()
      ])
    });

    // When race to value changes, update the racks validators
    this.scoreForm.get('raceTo')?.valueChanges.subscribe(() => {
      this.scoreForm.get('player1Racks')?.updateValueAndValidity();
      this.scoreForm.get('player2Racks')?.updateValueAndValidity();
      this.scoreForm.updateValueAndValidity();
    });

    // Watch for changes in rack values to trigger validation
    this.scoreForm.get('player1Racks')?.valueChanges.subscribe(() => {
      this.scoreForm.updateValueAndValidity();
    });

    this.scoreForm.get('player2Racks')?.valueChanges.subscribe(() => {
      this.scoreForm.updateValueAndValidity();
    });
  }


  private racksValidator() {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!this.scoreForm) return null;

      const raceTo = this.scoreForm.get('raceTo')?.value;
      if (raceTo === undefined) return null;

      const player1Racks = this.scoreForm.get('player1Racks')?.value;
      const player2Racks = this.scoreForm.get('player2Racks')?.value;
      if (player1Racks > raceTo || player2Racks > raceTo) {
        return { max: true };
      }

      if (player1Racks == player2Racks) {
        return { same: true }
      }
      return null;
    };
  }

  isMatchComplete(): boolean {
    const player1Racks = this.scoreForm.get('player1Racks')?.value || 0;
    const player2Racks = this.scoreForm.get('player2Racks')?.value || 0;
    const raceTo = this.scoreForm.get('raceTo')?.value || this.DefaultEightBallPoolRackValue;

    return (player1Racks < player2Racks && player2Racks == raceTo) || (player2Racks < player1Racks && player1Racks == raceTo);
  }

  async onSubmit() {
    if (this.scoreForm.valid) {
      const formValue = this.scoreForm.value;
      const score: EightBallPoolScoreType = {
        Player1Racks: formValue.player1Racks,
        Player2Racks: formValue.player2Racks,
        RaceTo: formValue.raceTo,
      };

      if (!this.isMatchComplete()) {
        this.scoreError.set("Neither player has reached the race to value");
        return;
      }

      // Clear the error
      this.scoreError.set(undefined);

      console.log('Submitting score:', score);

      await this.addMatchScore(score);
    }
  }
}
