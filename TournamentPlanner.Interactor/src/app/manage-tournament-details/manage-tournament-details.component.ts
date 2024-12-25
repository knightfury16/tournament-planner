import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { TournamentDto } from '../tp-model/TpModel';
import { TournamentPlannerService } from '../tournament-planner.service';
import { transformTournamentIsoDate } from '../../Shared/Utility/dateTimeUtility';

@Component({
  selector: 'app-manage-tournament-details',
  standalone: true,
  imports: [],
  templateUrl: './manage-tournament-details.component.html',
  styleUrl: './manage-tournament-details.component.scss'
})
export class ManageTournamentDetailsComponent implements OnInit {

  @Input({ required: true }) public tournamentId?: string;
  private _tpService = inject(TournamentPlannerService);

  public tournamentDetails = signal<TournamentDto | null>(null);
  @Output() public tournamentNameEE = new EventEmitter<string>();

  async ngOnInit() {
    if (this.tournamentId == undefined) { this.emitADummyName(); return }
    var tourDetail = await this._tpService.getTournamentById(this.tournamentId)
    var transformDateTournament = transformTournamentIsoDate(tourDetail);
    this.tournamentDetails.set(transformDateTournament);
    this.emitTournamentName(tourDetail.name);
  }
  emitTournamentName(tournamentName: string) {
    this.tournamentNameEE.emit(tournamentName);
  }

  emitADummyName() {
    this.tournamentNameEE.emit("Undefined Tournament");
  }
}
