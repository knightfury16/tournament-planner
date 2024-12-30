import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { GameTypeDto, MatchDto } from '../tp-model/TpModel';
import { MatchTabViewType } from '../tp-model/types';
import {  MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AddMatchScoreProviderComponent } from "../add-match-score-provider/add-match-score-provider.component";
import { TournamentPlannerService } from '../tournament-planner.service';

@Component({
  selector: 'app-add-match-score',
  standalone: true,
  imports: [MatCardModule, MatButtonModule, MatIconModule, AddMatchScoreProviderComponent],
  templateUrl: './add-match-score.component.html',
  styleUrl: './add-match-score.component.scss'
})
export class AddMatchScoreComponent implements OnInit {
  @Input({required: true}) public matchId?: number;
  @Input({required: true}) public gameType?: GameTypeDto | null;

  @Output() matchTabViewChangeEvent = new EventEmitter<MatchTabViewType>();

  public match = signal<MatchDto | undefined> (undefined);

  private _tpService = inject(TournamentPlannerService);

  async ngOnInit() {
    try {
      var response = await this._tpService.getMatchById(this.matchId!.toString());
      this.match.set(response);
    } catch (error) {
      console.log("ERROR FETCHING THE MATCH::", error);
    }
  }

  public emitMatchTabChangeEvent(){
    this.matchTabViewChangeEvent.emit(MatchTabViewType.MatchView);
  }


}
