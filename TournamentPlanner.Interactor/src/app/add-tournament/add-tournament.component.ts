import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TimeService } from '../time.service';
import { TournamentPlannerService } from '../tournament-planner.service';
import { Router } from '@angular/router';
import { AddTournamentDto, TournamentStatus, GameTypeSupported } from '../tp-model/TpModel';

@Component({
  selector: 'app-add-tournament',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-tournament.component.html',
  styleUrl: './add-tournament.component.scss',
})
export class AddTournamentComponent {
  public addTournamentDto: AddTournamentDto;
  public readonly gameTypeSupported = Object.values(GameTypeSupported);
  public readonly tournamentStatus = Object.values(TournamentStatus);

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
    if (registrationLastDate && startDate && new Date(registrationLastDate) > new Date(startDate)) {
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
