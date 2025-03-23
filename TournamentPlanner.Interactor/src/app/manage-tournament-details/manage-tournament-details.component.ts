import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { PlayerDto, TournamentDto, TournamentStatus, TournamentStatusColor, TournamentType } from '../tp-model/TpModel';
import { TournamentPlannerService } from '../tournament-planner.service';
import { transformTournamentIsoDate } from '../../Shared/Utility/dateTimeUtility';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { mapStringToEnum, trimAllSpace } from '../../Shared/Utility/stringUtility';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { SnackbarService } from '../../Shared/snackbar.service';
import { AdminTournamentService } from '../../Shared/admin-tournament.service';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatChipsModule } from '@angular/material/chips';
import { RouterModule } from '@angular/router';
import { TournamentColorService } from '../../Shared/tournament-color.service';

@Component({
  selector: 'app-manage-tournament-details',
  standalone: true,
  imports: [
    MatFormFieldModule,
    ReactiveFormsModule,
    CommonModule,
    MatSelectModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatChipsModule,
    RouterModule
  ],
  templateUrl: './manage-tournament-details.component.html',
  styleUrl: './manage-tournament-details.component.scss'
})
export class ManageTournamentDetailsComponent implements OnInit {

  @Input({ required: true }) public tournamentId?: string;
  @Output() public tournamentEE = new EventEmitter<TournamentDto>();
  @Output() public participantsEE = new EventEmitter<PlayerDto[]>();
  @Output() public statusChangeEE = new EventEmitter();

  public tournamentStatus = TournamentStatus;
  private _tpService = inject(TournamentPlannerService);
  private _adminTpService = inject(AdminTournamentService);
  private _snackBarService = inject(SnackbarService);

  public tournamentDetails = signal<TournamentDto | null>(null);
  public selectedStatus: TournamentStatus | undefined = undefined;
  public statusFormControl = new FormControl();
  public tournamentColorService = inject(TournamentColorService);

  async ngOnInit() {
    if (this.tournamentId == undefined) { return; }
    var tourDetail = await this.fetchTournament(this.tournamentId);
    this.emitTournament(tourDetail);// name will be here
    this.setSelectedStatus();
  }

  async fetchTournament(tournamentId: string) {
    var tourDetail = await this._tpService.getTournamentById(tournamentId)
    var transformDateTournament = transformTournamentIsoDate(tourDetail);
    this.emitTournamentParticipants(tourDetail.participants);
    this.tournamentDetails.set(transformDateTournament);
    return tourDetail;
  }


  private emitTournamentParticipants(participants: PlayerDto[] | undefined) {
    if (participants && participants.length > 0) {
      this.participantsEE.emit(participants);
    }
  }

  setSelectedStatus() {
    this.selectedStatus = this.getStatusValue();
  }
  emitTournament(tournament: TournamentDto) {
    this.tournamentEE.emit(tournament);
  }

  getStatusValue(): TournamentStatus {
    var status = this.tournamentDetails()?.status ?? TournamentStatus.Draft.toString();
    return mapStringToEnum(TournamentStatus, status);
  }

  getTournamentStatusString(): string[] {
    // I have to do this hack because if there is space in enum values all hell break loose
    //ts can not map it properly
    var status: string[] = [];
    Object.keys(TournamentStatus).forEach(
      key => {
        if (!isNaN(Number(key))) return;
        status.push(trimAllSpace(TournamentStatus[key as keyof typeof TournamentStatus]));
      }
    )
    return status;
  }
  public async changeStatus() {
    try {
      var changedStatus = this.statusFormControl.value;
      var statusChangeResponse = await this._adminTpService.changeTournamentStatus(this.tournamentId!, changedStatus);
      this.statusChangeEE.emit();
      console.log(statusChangeResponse);
      await this.fetchTournament(this.tournamentId!);
      this.statusFormControl.reset();
      this._snackBarService.showMessage(statusChangeResponse.message, 4);
    } catch (error: any) {
      this.statusFormControl.reset();
      console.log(error!.error);
      this._snackBarService.showError((error as any).error ?? (error as any).error?.Error ?? "An unknown error occurred.");
    }
  }

  formatDate(date: string | null | undefined): string {
    if (!date) return 'Not set';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  getStatusColor(status: string): string {
    return this.tournamentColorService.getTournamentStatusColor(status);
  }

  public isGroup(): boolean {
    if (this.tournamentDetails() != undefined) return this.tournamentDetails()?.tournamentType == trimAllSpace(TournamentType.GroupStage);
    return false
  }

}
