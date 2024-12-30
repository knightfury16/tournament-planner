import { Component, OnInit, signal } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { BaseAddScoreComponent } from '../base-add-score/base-add-score.component';
import { SetScore } from '../../../GameTypeSuportedScore/TableTennisScore';
import { TableTennisScoreType } from '../../../GameTypeSuportedScore/TableTennisScore';

@Component({
  selector: 'app-table-tennis-add-score',
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
  templateUrl: './table-tennis-add-score.component.html',
  styleUrls: ['./table-tennis-add-score.component.scss']
})
export class TableTennisAddScoreComponent extends BaseAddScoreComponent {
  public scoreError = signal<string | undefined>(undefined);
  public scoreForm?: FormGroup;
  public setsToWin = 2;
  public pointsPerSet = 11;

  public currentInput = 3;
  constructor() {
    super();
    this.initializeForm();
  }


  private initializeForm() {
    this.scoreForm = new FormGroup({
      setsToWin: new FormControl(this.setsToWin, [Validators.required, Validators.min(1), Validators.max(7)]),
      pointsPerSet: new FormControl(this.pointsPerSet, [Validators.required, Validators.min(1)]),
      sets: new FormArray([])
    });

    // Initialize with default 3 sets
    this.initiateFormArray(this.currentInput);
  }

  private initiateFormArray(inputCount: number) {
    const setsArray = this.scoreForm?.get('sets') as FormArray;

    // Add new set groups
    for (let i = 0; i < inputCount; i++) {
      setsArray.push(this.createSetGroup());
    }
  }

  private createSetGroup(): FormGroup {
    const group = new FormGroup({
      player1Points: new FormControl(0, [
        Validators.required,
        Validators.min(0),
        this.pointsValidator.bind(this)
      ]),
      player2Points: new FormControl(0, [
        Validators.required,
        Validators.min(0),
        this.pointsValidator.bind(this)
      ])
    });

    // Add cross-validation between player points
    group.valueChanges.subscribe(() => {
      const player1Control = group.get('player1Points');
      const player2Control = group.get('player2Points');

      if (player1Control && player2Control) {
        player1Control.updateValueAndValidity({ emitEvent: false });
        player2Control.updateValueAndValidity({ emitEvent: false });
      }
    });

    return group;
  }

  private pointsValidator(control: AbstractControl) {
    if (!control.parent) return null;

    const group = control.parent as FormGroup;
    const player1Points = group.get('player1Points')?.value || 0;
    const player2Points = group.get('player2Points')?.value || 0;
    const pointsPerSet = this.scoreForm?.get('pointsPerSet')?.value || 11;

    // Check if either player has reached minimum points to win
    const maxPoints = Math.max(player1Points, player2Points);
    if (maxPoints < pointsPerSet) return { minimumPointsNotReached: true };

    // Calculate point difference
    const pointDifference = Math.abs(player1Points - player2Points);

    // Validate 2-point difference rule
    if (maxPoints >= pointsPerSet && pointDifference < 2) {
      return { insufficientDifference: true };
    }

    // Validate excessive points
    const maxAllowedPoints = Math.max(pointsPerSet, Math.min(player1Points, player2Points) + 2);
    if (maxPoints > maxAllowedPoints) {
      return { excessivePoints: true };
    }

    return null;
  }

  getSets() {
    return (this.scoreForm?.get('sets') as FormArray).controls;
  }

  getPointsPerSetValue() {
    return this.scoreForm?.get("pointsPerSet")?.value;
  }

  getSetToWinValue() {
    return this.scoreForm?.get("setsToWin")?.value;

  }

  addInput() {
    this.currentInput++;
    const setsArray = this.scoreForm?.get('sets') as FormArray;
    setsArray.push(this.createSetGroup());
  }
  getAddInputDisable() {
    return this.currentInput >= this.getSetToWinValue() + 2;
  }

  getSetNumber(index: number): number {
    return index + 1;
  }

  onSubmit() {
    if (this.addScoreForm.valid) {
      const formValue = this.scoreForm?.value;
      const score: TableTennisScoreType = {
        Player1Sets: 0,
        Player2Sets: 0,
        SetsToWin: formValue.setsToWin,
        PointsPerSet: formValue.pointsPerSet,
        SetScores: formValue.sets.map((set: any) => ({
          Player1Points: set.player1Points,
          Player2Points: set.player2Points
        })),
      };

      // Calculate sets won
      score.SetScores.forEach((set: SetScore) => {
        if (set.Player1Points > set.Player2Points) {
          score.Player1Sets++;
        } else if (set.Player2Points > set.Player1Points) {
          score.Player2Sets++;
        }
      });

      // Check if match is complete
      score.IsComplete = score.Player1Sets >= score.SetsToWin ||
        score.Player2Sets >= score.SetsToWin;

      if (!score.IsComplete) { this.scoreError.set("Minimum set to win not achived by either player"); return; }
      //clear the error
      this.scoreError.set(undefined);

      // TODO: Send to backend
      console.log('Submitting score:', score);
    }
  }


  isSetComplete(setGroup: AbstractControl): boolean {
    if (!setGroup.valid) return false;

    const player1Points = setGroup.get('player1Points')?.value || 0;
    const player2Points = setGroup.get('player2Points')?.value || 0;
    const pointsPerSet = this.addScoreForm.get('pointsPerSet')?.value || 11;
    const pointDifference = Math.abs(player1Points - player2Points);

    return Math.max(player1Points, player2Points) >= pointsPerSet &&
      pointDifference >= 2;
  }
}