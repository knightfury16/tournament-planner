import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TournamentPlannerService } from '../tournament-planner.service';
import { Router } from '@angular/router';
import { AddTournamentDto, TournamentStatus, GameTypeSupported, ResolutionStrategy, TournamentType, GameTypeDto, KnocoutMatchTypeMaxParticipant, GroupMatchTypePPG, PlayerAdvanceFromPerGroup } from '../tp-model/TpModel';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatOptionModule, provideNativeDateAdapter } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { LoadingService } from '../../Shared/loading.service';
import { firstValueFrom } from 'rxjs';
import { trimAllSpace } from '../../Shared/Utility/stringUtility';

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
export class AddTournamentComponent implements OnInit {
  public addTournamentDto: AddTournamentDto | null = null;
  public readonly gameTypeSupported = GameTypeSupported;
  public readonly tournamentStatus = TournamentStatus;
  public readonly tournamentType = TournamentType;
  public loadingService = inject(LoadingService);
  public errors = signal<string[] | null>(null);
  public showKnockoutField = signal<boolean>(false);
  public maxParticipantLimit = signal<number | null>(null);
  public addTournamentForm: FormGroup;
  public today: Date;
  public minStartDate: Date;
  public minEndDate: Date;

  constructor(
    private tp: TournamentPlannerService,
    private router: Router
  ) {
    this.today = new Date();
    this.minStartDate = new Date();
    this.minStartDate.setDate(this.today.getDate() + 2); //adding 2 days because i need to have a registratin last date

    this.minEndDate = new Date();
    this.minEndDate.setDate(this.minStartDate.getDate() + 1); //adding 1 more day with the start day by default

    this.addTournamentForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
      startDate: new FormControl<Date>(this.minStartDate, [Validators.required]),
      endDate: new FormControl<Date>(this.minEndDate, [Validators.required]),
      gameType: new FormControl<GameTypeSupported | null>(null, [Validators.required]),
      status: new FormControl<TournamentStatus>(TournamentStatus.Draft, [Validators.required]),
      registrationLastDate: new FormControl<Date | null>(null, [this.registrationLastDateValidator.bind(this)]),
      maxParticipant: new FormControl<string>('', [this.maxParticipantValidator.bind(this)]),
      venue: new FormControl<string>(''),
      registrationFee: new FormControl<string>(''),
      minimumAgeOfRegistration: new FormControl<number | null>(null),
      knockOutStartNumber: new FormControl<number>(8, [Validators.required]),
      tournamentType: new FormControl<TournamentType | null>(null, [Validators.required]),
    });
  }

  ngOnInit(): void {
    this.UpdateKnockoutFieldVisibility();

    this.addTournamentForm.get('tournamentType')?.valueChanges.subscribe(value => {
      this.UpdateKnockoutFieldVisibility();
      this.UpdateMaxCountLimit();
    })
    this.addTournamentForm.get('knockOutStartNumber')?.valueChanges.subscribe(_ => {
      this.UpdateMaxCountLimit();
    })
    this.addTournamentForm.get('startDate')?.valueChanges.subscribe(_ => {
      this.validateRegistrationLastDate();
    })
  }


  public UpdateKnockoutFieldVisibility(): void {
    var tournamentTypeValue = this.addTournamentForm.get('tournamentType')?.value;
    this.showKnockoutField.set(tournamentTypeValue === trimAllSpace(this.tournamentType.GroupStage));
  }

  public UpdateMaxCountLimit(): void {

    var tournamentTypeValue = this.addTournamentForm.get('tournamentType')?.value;
    var knockOutStartNumberValue = this.addTournamentForm.get('knockOutStartNumber')?.value;
    var currentMaxParticipantValue = this.addTournamentForm.get('maxParticipant')?.value;
    var currentMaxParticipantNumber = currentMaxParticipantValue ? parseInt(currentMaxParticipantValue) : null;

    if (tournamentTypeValue == trimAllSpace(this.tournamentType.Knockout)) {
      this.maxParticipantLimit.set(KnocoutMatchTypeMaxParticipant);
    }

    else if (tournamentTypeValue == trimAllSpace(this.tournamentType.GroupStage) && knockOutStartNumberValue) {
      var maxLimit = (GroupMatchTypePPG / PlayerAdvanceFromPerGroup) * knockOutStartNumberValue;
      this.maxParticipantLimit.set(maxLimit);
    }

    // Update the maxParticipant field value if needed
    if (this.maxParticipantLimit() !== null) {
      if (!currentMaxParticipantValue ||
        (currentMaxParticipantNumber && currentMaxParticipantNumber > this.maxParticipantLimit()!)) {
        this.addTournamentForm.get('maxParticipant')?.setValue(this.maxParticipantLimit()!.toString());
      }
    }
  }

  // Custom validator for maxParticipant
  public maxParticipantValidator(control: AbstractControl) {
    const value = control.value ? parseInt(control.value) : null;
    const limit = this.maxParticipantLimit();

    if (value && limit && value > limit) {
      return { 'exceedsMaxLimit': true };
    }
    return null;
  }

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
    if (registrationLastDate == null) return null; //since this is nullbale
    if (registrationLastDate && startDate && new Date(registrationLastDate) >= new Date() && new Date(registrationLastDate) < new Date(startDate)) {
      return null;
    }
    return { 'invalidRegistrationLastDate': true };
  }

  validateRegistrationLastDate() {
    const control = this.addTournamentForm.get("registrationLastDate");
    if (control) {
      control.updateValueAndValidity();
    }
  }

  // Function to disable past dates
  // I can also disable date with my custom fuction
  disablePastDates = (date: Date | null): boolean => {
    if (!date) return false;
    const today = new Date();
    today.setDate(today.getDate() + 1);
    today.setHours(0, 0, 0, 0); // Set time to midnight to compare only dates
    return date >= today;
  };

  public getKnockoutStartNumber(): number[] {
    return [8, 16, 32, 64];
  }

}
