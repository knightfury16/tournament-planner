// make-match-schedule.component.ts
import { Component, inject, Input, OnInit, signal } from '@angular/core';
import { AdminTournamentService } from '../../Shared/admin-tournament.service';
import { LoadingService } from '../../Shared/loading.service';
import { MatButtonModule } from '@angular/material/button';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';

interface ScheduleForm {
  eachMatchTime: string;
  startTime: string;
}

@Component({
  selector: 'app-make-match-schedule',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    ReactiveFormsModule
  ],
  templateUrl: './make-match-schedule.component.html',
  styleUrl: './make-match-schedule.component.scss'
})
export class MakeMatchScheduleComponent implements OnInit {
  @Input({ required: true }) public tournamentId?: string;
  public canISchedule = signal(false);
  public scheduleForm: FormGroup;

  private _adminService = inject(AdminTournamentService);
  private _loadingService = inject(LoadingService);
  private _fb = inject(FormBuilder);

  constructor() {
    this.scheduleForm = this._fb.group({
      eachMatchTime: ['30m', [Validators.required, Validators.pattern('^[0-9]+[mh]$')]],
      startTime: ['10:10:00', [Validators.required, Validators.pattern('^([0-1]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$')]]
    });
  }

  async ngOnInit() {
    try {
      this._loadingService.show();
      const canIScheduleDto = await this._adminService.canISchedule(this.tournamentId!.toString());
      console.log("CAN I SCHEDULE RESPONSE:::", canIScheduleDto);
      this.canISchedule.set(canIScheduleDto.success);
    } catch (err: any) {
      throw new Error(err);
    } finally {
      this._loadingService.hide();
    }
  }

  async onSubmit() {
    if (this.scheduleForm.valid && this.canISchedule()) {
      try {
        this._loadingService.show();
        const scheduleData: ScheduleForm = this.scheduleForm.value;
        // Assuming your service has a method to submit the schedule
        // await this._adminService.scheduleMatches(this.tournamentId!, scheduleData);
        // Handle success (maybe show a snackbar or navigate somewhere)
      } catch (error: any) {
        // Handle error (show error message)
        console.error('Error scheduling matches:', error);
      } finally {
        this._loadingService.hide();
      }
    }
  }

  getErrorMessage(controlName: string): string {
    const control = this.scheduleForm.get(controlName);
    if (control?.hasError('required')) {
      return 'This field is required';
    }
    if (control?.hasError('pattern')) {
      if (controlName === 'eachMatchTime') {
        return 'Format should be like "30m" or "1h"';
      }
      return 'Invalid time format (HH:MM:SS)';
    }
    return '';
  }
}