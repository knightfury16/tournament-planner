import { Component, EventEmitter, inject, Input, OnInit, Output, output, signal } from '@angular/core';
import { TournamentPlannerService } from '../tournament-planner.service';
import { DomainRole, PlayerDto, TournamentDto } from '../tp-model/TpModel';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../Shared/auth.service';
import { LoadingService } from '../../Shared/loading.service';
import { SnackbarService } from '../../Shared/snackbar.service';

@Component({
  selector: 'app-tournament-details',
  standalone: true,
  imports: [MatButtonModule],
  templateUrl: './tournament-details.component.html',
  styleUrl: './tournament-details.component.scss'
})
export class TournamentDetailsComponent implements OnInit {
  @Input({required: true}) public tournamentId?: string;

  @Output() public tournamentNameEE = new EventEmitter<string>();
  @Output() public participantsEE = new EventEmitter<PlayerDto[]>();

  private _tpService = inject(TournamentPlannerService);
  private _snackBar = inject(SnackbarService);
  public authService = inject(AuthService);
  public loadingService = inject(LoadingService);

  public tournamentDetails = signal<TournamentDto | null>(null);
  public canRegister = false;

  async ngOnInit() {
    if (this.tournamentId == undefined) { this.emitADummyName(); return }
    var tourDetail = await this._tpService.getTournamentById(this.tournamentId)
    this.tournamentDetails.set(tourDetail);
    this.emitTournamentName(tourDetail.name);
    this.emitToutnamentParticipants(tourDetail.participants);
    this.setCanRegister();

  }
  setCanRegister() {
    var currentUser = this.authService.getCurrentUser();
    if (currentUser && currentUser.role == DomainRole.Player) {
      this.canRegister = true;
    }
  }

  emitToutnamentParticipants(participants: PlayerDto[] | undefined) {
    if(participants && participants.length > 0)
    {
      this.participantsEE.emit(participants);
    }
  }
  emitTournamentName(tournamentName: string) {
    this.tournamentNameEE.emit(tournamentName);
  }

  emitADummyName() {
    this.tournamentNameEE.emit("Undefined Tournament");
  }

  public async registerInTournament() {
    try {

      var currentUser = this.authService.getCurrentUser();
      var currentUseremail = currentUser?.email;

      if (this.checkIfAlreadyRegistered(currentUseremail)) { this._snackBar.showWarning("You are already registered for this tournament."); return }
      if (this.tournamentParticipantsFull()) { this._snackBar.showWarning("Tournament Participants is full"); return; }

      this.loadingService.show();
      await this._tpService.registerPlayerInTournament(this.tournamentId!);
      this.loadingService.hide();
      this.showSuccesMessage();
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
      return tournamentDetails.participants.length >= tournamentDetails.maxParticipants;
    }
    return false;
  }

  showSuccesMessage() {
    this._snackBar.showMessage("Registered Successfully.");
  }

  checkIfAlreadyRegistered(currentUseremail: string | undefined) {
    if (!currentUseremail) return false;
    const tournamentDetails = this.tournamentDetails();
    if (tournamentDetails && tournamentDetails.participants) {
      return tournamentDetails.participants.find(participant => participant.email === currentUseremail) !== undefined;
    }
    return false;
  }

}
