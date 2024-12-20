import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TournamentPlannerService } from '../tournament-planner.service';
import { Router } from '@angular/router';
import { AddTournamentDto, TournamentStatus, GameTypeSupported, ResolutionStrategy, TournamentType, GameTypeDto } from '../tp-model/TpModel';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatOptionModule, provideNativeDateAdapter } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { LoadingService } from '../../Shared/loading.service';
import { firstValueFrom } from 'rxjs';

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
  public addTournamentDto: AddTournamentDto | null = null;
  public readonly gameTypeSupported = GameTypeSupported;
  public readonly tournamentStatus = TournamentStatus;
  public readonly tournamentType = TournamentType;
  public loadingService = inject(LoadingService);
  public errors = signal<string[] | null>(null);


  constructor(
    private tp: TournamentPlannerService,
    private router: Router
  ) { }

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
    tournamentType: new FormControl<TournamentType | null>(null, [Validators.required]),
  });

  public async onClickCreate() {
    if (this.addTournamentForm.valid) {
      this.addTournamentDto = {
        name: this.addTournamentForm.value.name ?? "",
        startDate: this.addTournamentForm.value.startDate?.toISOString() ?? null,
        endDate: this.addTournamentForm.value.endDate?.toISOString() ?? null,
        gameType: this.getGameTypeDto(this.addTournamentForm.value.gameType ?? null),
        status: this.addTournamentForm.value.status ?? undefined,
        registrationLastDate: this.addTournamentForm.value.registrationLastDate?.toISOString() ?? undefined,
        maxParticipant: this.addTournamentForm.value.maxParticipant ? parseInt(this.addTournamentForm.value.maxParticipant) : undefined,
        venue: this.addTournamentForm.value.venue ?? undefined,
        registrationFee: this.addTournamentForm.value.registrationFee ? parseInt(this.addTournamentForm.value.registrationFee) : undefined,
        minimumAgeOfRegistration: this.addTournamentForm.value.minimumAgeOfRegistration ? this.addTournamentForm.value.minimumAgeOfRegistration : undefined,
        knockOutStartNumber: this.addTournamentForm.value.knockOutStartNumber ?? undefined,
        winnerPerGroup: 2, // locking it to 2
        participantResolutionStrategy: ResolutionStrategy.StatBased, // Assuming a default strategy
        tournamentType: this.addTournamentForm.value.tournamentType ?? this.tournamentType.GroupStage,
      };
      console.log(JSON.stringify(this.addTournamentDto));

      await this.createTournament(this.addTournamentDto);
      this.router.navigate(['/tp']);

      console.log(this.addTournamentDto);
    }
  }
  async createTournament(addTournamentDto: AddTournamentDto) {
    this.loadingService.show();
    try {
      await firstValueFrom(this.tp.addTournament(addTournamentDto))
      this.loadingService.hide();
      this.addTournamentForm.reset();
    } catch (error: any) {
      const errors = error.error?.errors ? Object.values(error.error.errors).flat() : error.error?.Error ? [error.error.Error] : [];
      this.errors.set(errors);
      this.loadingService.hide();
    }
  }
  getGameTypeDto(gameType: GameTypeSupported | null): GameTypeDto | null {
    if (gameType) {
      return {
        name: gameType
      };
    }
    return null;
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
