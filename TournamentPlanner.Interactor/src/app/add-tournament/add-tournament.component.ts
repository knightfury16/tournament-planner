import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TimeService } from '../time.service';
import { TournamentPlannerService } from '../tournament-planner.service';
import { Router } from '@angular/router';
import { AddTournamentDto, TournamentStatus, GameTypeSupported } from '../tp-model/TpModel';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatOptionModule, provideNativeDateAdapter } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-add-tournament',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [CommonModule, FormsModule, ReactiveFormsModule,
    MatFormFieldModule, MatButtonModule,
    MatCardModule, MatInputModule, MatDatepickerModule, MatOptionModule, MatSelectModule],
  templateUrl: './add-tournament.component.html',
  styleUrl: './add-tournament.component.scss',
})
export class AddTournamentComponent {
  public addTournamentDto: AddTournamentDto;
  public readonly gameTypeSupported = GameTypeSupported;
  public readonly tournamentStatus = TournamentStatus;

  constructor(
    private tp: TournamentPlannerService,
    private timeService: TimeService,
    private router: Router
  ) {
    this.addTournamentDto = {
      name: '',
      status: TournamentStatus.Draft,
      startDate: new Date().toUTCString(),
      endDate: new Date().toUTCString(),
      gameType: GameTypeSupported.TableTennis,
      knockOutStartNumber: 8,
    };
  }

  public addTournamentForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    startDate: new FormControl<Date>(new Date(), [Validators.required]),
    endDate: new FormControl<Date>(new Date(), [Validators.required]),
    gameType: new FormControl<GameTypeSupported | null>(null, [Validators.required]),
    status: new FormControl<TournamentStatus>(TournamentStatus.Draft, [Validators.required]),
    registrationLastDate: new FormControl<Date | null>(null, [this.registrationLastDateValidator]),
    maxParticipant: new FormControl<string>(''),
    venue: new FormControl<string>(''),
    registrationFee: new FormControl<string>(''),
    minimumAgeOfRegistration: new FormControl<number | null>(null),
    knockOutStartNumber: new FormControl<number>(8, [Validators.required, this.powerOfTwoValidator]),
  });

  onClickCreate() {
    this.tp.addTournament(this.addTournamentDto).subscribe((res) => {
      console.log(res);
      this.router.navigate(['/tp'])
    });
  }

  onStartDateChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const localDate = input.value;
    this.addTournamentDto.startDate = this.timeService.convertToUTC(localDate);
  }
  onEndDateChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const localDate = input.value;
    this.addTournamentDto.endDate = this.timeService.convertToUTC(localDate);
  }

  private registrationLastDateValidator(control: AbstractControl) {
    const registrationLastDate = control.value
    var val = new Date(registrationLastDate);
    const startDate = control.parent?.get('startDate')?.value;
    if (registrationLastDate && startDate && new Date(registrationLastDate) >= new Date() && new Date(registrationLastDate) < new Date(startDate)) {
      return null;
    }
    return { 'invalidRegistrationLastDate': true };
  }

  private powerOfTwoValidator(control: AbstractControl) {
    const value = parseInt(control.value);
    if (value && value > 0 && (value & (value - 1)) === 0) {
      return null;
    }
    return { 'invalidPowerOfTwo': true };
  }
}
