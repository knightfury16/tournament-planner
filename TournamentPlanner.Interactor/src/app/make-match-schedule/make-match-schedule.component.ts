// make-match-schedule.component.ts
import { Component, effect, inject, Input, OnChanges, OnInit, signal, SimpleChanges } from '@angular/core';
import { AdminTournamentService } from '../../Shared/admin-tournament.service';
import { LoadingService } from '../../Shared/loading.service';
import { MatButtonModule } from '@angular/material/button';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { ScheduleingInfo, TournamentDto } from '../tp-model/TpModel';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { provideNativeDateAdapter } from '@angular/material/core';
import { getDateTimeWithTimeZoneOffsetToMidnight } from '../../Shared/Utility/dateTimeUtility';

interface ScheduleForm {
  eachMatchTime: string;
  startTime: string;
}

@Component({
  selector: 'app-make-match-schedule',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatSelectModule
  ],
  templateUrl: './make-match-schedule.component.html',
  styleUrl: './make-match-schedule.component.scss'
})
export class MakeMatchScheduleComponent implements OnInit, OnChanges {
  @Input({ required: true }) tournament?: TournamentDto;
  public canISchedule = signal(false);
  private _tournament = signal<TournamentDto | undefined>(undefined);

  private _adminService = inject(AdminTournamentService);
  private _loadingService = inject(LoadingService);


  public scheduleForm = new FormGroup({
    startDate: new FormControl(this.getTournamentDefaultStartDate(), [Validators.required]),
    startTime: new FormControl('10:00', [Validators.required]),
    endTime: new FormControl('17:00', [Validators.required]),
    eachMatchTime: new FormControl<number>(30, [Validators.required]),
    matchPerDay: new FormControl<number>(15, [Validators.required]),
    parallelMatch: new FormControl<number>(1, [Validators.required])
  });

  constructor() {
    // Update form's start date when tournament changes
    effect(() => {
      if (this._tournament()) {
        this.scheduleForm.patchValue({
          startDate: this.getTournamentDefaultStartDate()
        });
      }
    });
  }

  async ngOnInit() {
    if (this.tournament) {
      await this.updateSchedulePermission()
    }
  }
  async ngOnChanges(changes: SimpleChanges) {
    if (changes['tournament'] && changes['tournament'].currentValue) {
      this._tournament.set(changes['tournament'].currentValue);
      await this.updateSchedulePermission();
    }
  }

  private async updateSchedulePermission() {
    try {
      if (this.tournament == null) return;
      this._loadingService.show();
      const canIScheduleDto = await this._adminService.canISchedule(this.tournament!.id.toString());
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
        const scheduleData: ScheduleingInfo = {
          startDate: this.scheduleForm.value.startDate ? getDateTimeWithTimeZoneOffsetToMidnight(this.scheduleForm.value.startDate) : '',
          startTime: this.scheduleForm.value.startTime ?? '10:00',
          endTime: this.scheduleForm.value.endTime ?? '17:00',
          eachMatchTime: this.scheduleForm.value.eachMatchTime?.toString() ?? '30',
          matchPerDay: this.scheduleForm.value.matchPerDay ?? 15,
          parallelMatchesPossible: this.scheduleForm.value.parallelMatch ?? 1
        }
        console.log("SCHEDULE DATA:::", scheduleData);
      } catch (error: any) {
        // Handle error (show error message)
        console.error('Error scheduling matches:', error);
      } finally {
        this._loadingService.hide();
      }
    }
  }

  getTournamentDefaultStartDate(): Date | null {
    const tournament = this._tournament();
    if (!tournament?.startDate) return null;

    const dateString = tournament.startDate.toString();
    return new Date(dateString);
  }

  public getStartTimeOptions(): string[] {
    var times: string[] = []
    for (let i = 1; i <= 24; i++) {
      if (i < 10) times.push(`0${i}:00`);
      else times.push(`${i}:00`);
    }
    return times;
  }

  public getEndTimeOptions(): string[] {
    //if needed configure end time separately
    return this.getStartTimeOptions();
  }

  getErrorMessage(controlName: string): string {
    const control = this.scheduleForm.get(controlName);

    //TODO: debug here. endtime validation not working
    if(control && controlName == "endTime")
    {
      const startTime = this.scheduleForm.get("startTime")?.value;
      if(startTime && control.value <= startTime)
      {
        return 'End time should be greater than start time';
      }
    }

    if (control?.hasError('required')) {
      return 'This field is required';
    }
    return '';
  }
}