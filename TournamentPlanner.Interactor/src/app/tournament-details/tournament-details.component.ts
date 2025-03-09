import { Component, EventEmitter, inject, Input, OnInit, Output, output, signal } from '@angular/core';
import { TournamentPlannerService } from '../tournament-planner.service';
import { DomainRole, PlayerDto, TournamentDto, TournamentStatus } from '../tp-model/TpModel';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../Shared/auth.service';
import { LoadingService } from '../../Shared/loading.service';
import { SnackbarService } from '../../Shared/snackbar.service';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CommonModule } from '@angular/common';
import { trimAllSpace } from '../../Shared/Utility/stringUtility';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-tournament-details',
  standalone: true,
  imports: [RouterModule, MatButtonModule, MatCardModule, MatChipsModule, MatIconModule, MatDividerModule, MatTooltipModule, CommonModule],
  templateUrl: './tournament-details.component.html',
  styleUrl: './tournament-details.component.scss'
})
export class TournamentDetailsComponent implements OnInit {
  @Input({ required: true }) public tournamentId?: string;

  @Output() public tournamentEE = new EventEmitter<TournamentDto>();
  @Output() public participantsEE = new EventEmitter<PlayerDto[]>();

  private _tpService = inject(TournamentPlannerService);
  private _snackBar = inject(SnackbarService);
  public authService = inject(AuthService);
  public loadingService = inject(LoadingService);

  public tournamentDetails = signal<TournamentDto | null>(null);
  public canRegister = false;
  public isRegistered = true;

  async ngOnInit() {
    if (this.tournamentId == undefined) { return }
    var tourDetail = await this._tpService.getTournamentById(this.tournamentId)
    this.tournamentDetails.set(tourDetail);
    this.emtiTournament(tourDetail); //name will be here
    this.emitToutnamentParticipants(tourDetail.participants);
    this.isRegistered = this.checkIfAlreadyRegistered();
    this.setCanRegister();

  }
  setCanRegister() {
    var currentUser = this.authService.getCurrentUser();
    if (currentUser && currentUser.role == DomainRole.Player && this.registrationOpen()) {
      this.canRegister = true;
    }
  }

  public registrationOpen(): boolean {
    const tournamentDetails = this.tournamentDetails();
    if (tournamentDetails && tournamentDetails.status) {
      return tournamentDetails.status == trimAllSpace(TournamentStatus.RegistrationOpen)
    }
    return false;
  }

  emitToutnamentParticipants(participants: PlayerDto[] | undefined) {
    if (participants && participants.length > 0) {
      this.participantsEE.emit(participants);
    }
  }
  emtiTournament(tournament: TournamentDto) {
    this.tournamentEE.emit(tournament);
  }


  public async registerInTournament() {
    try {
      if (this.checkIfAlreadyRegistered()) { this._snackBar.showWarning("You are already registered for this tournament."); return }
      if (this.tournamentParticipantsFull()) { this._snackBar.showWarning("Tournament Participants is full"); return; }

      this.loadingService.show();
      await this._tpService.registerPlayerInTournament(this.tournamentId!);
      this.loadingService.hide();
      this.showSuccesMessage();
      this.isRegistered = true;
      //fetch the participant and emit it again to refresh the new player
      await this.refreshTheTournamentParticipants();

    } catch (error: any) {
      this.loadingService.hide();
      this._snackBar.showError(error.error!.Error)
      console.log(error);
    }
  }
  private async refreshTheTournamentParticipants() {
    var playerResponse = await this._tpService.getTournamentPlayers(this.tournamentId!);
    this.tournamentDetails()!.participants = playerResponse;
    console.log("Participants updated.....", playerResponse);
    this.emitToutnamentParticipants(playerResponse);
  }


  tournamentParticipantsFull(): boolean {
    const tournamentDetails = this.tournamentDetails();
    if (tournamentDetails && tournamentDetails.participants) {
      return tournamentDetails.participants.length >= tournamentDetails.maxParticipant;
    }
    return false;
  }

  showSuccesMessage() {
    this._snackBar.showMessage("Registered Successfully.");
  }

  checkIfAlreadyRegistered() {
    var currentUser = this.authService.getCurrentUser();
    var currentUseremail = currentUser?.email;

    if (!currentUseremail) return false;
    const tournamentDetails = this.tournamentDetails();
    if (tournamentDetails && tournamentDetails.participants) {
      return tournamentDetails.participants.find(participant => participant.email === currentUseremail) !== undefined;
    }
    return false;
  }

  getStatusColor(status: string | null | undefined): string {
    if (!status) return '';
    switch (status) {
      case TournamentStatus.Draft:
        return 'gray';
      case trimAllSpace(TournamentStatus.RegistrationOpen):
        return 'green';
      case trimAllSpace(TournamentStatus.RegistrationClosed):
        return 'orange';
      case TournamentStatus.Ongoing:
        return 'blue';
      case TournamentStatus.Completed:
        return 'purple';
      default:
        return 'gray';
    }
  }
  getParticipantsProgress(): number {
    const tournament = this.tournamentDetails();
    if (!tournament) return 0;
    return ((tournament.participants?.length || 0) / tournament.maxParticipant) * 100;
  }
  formatDate(date: string | null | undefined): string {
    if (!date) return 'Not set';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }
}
