import { Component, EventEmitter, inject, Input, OnInit, Output, output, signal } from '@angular/core';
import { TournamentPlannerService } from '../tournament-planner.service';
import { PlayerDto, TournamentDto } from '../tp-model/TpModel';

@Component({
  selector: 'app-tournament-details',
  standalone: true,
  imports: [],
  templateUrl: './tournament-details.component.html',
  styleUrl: './tournament-details.component.scss'
})
export class TournamentDetailsComponent implements OnInit {
  @Input({required: true}) public tournamentId?: string;

  @Output() public tournamentNameEE = new EventEmitter<string>();
  @Output() public participantsEE = new EventEmitter<PlayerDto[]>();

  public tpService = inject(TournamentPlannerService);

  public tournamentDetails = signal<TournamentDto | null>(null);

  async ngOnInit() {
    if (this.tournamentId == undefined) { this.emitADummyName(); return }
    var tourDetail = await this.tpService.getTournamentById(this.tournamentId)
    this.tournamentDetails.set(tourDetail);
    this.emitTournamentName(tourDetail.name);
    this.emitToutnamentParticipants(tourDetail.participants);

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


}
