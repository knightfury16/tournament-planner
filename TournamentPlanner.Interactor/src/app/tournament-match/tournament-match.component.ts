import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { MatchModel } from '../tournament-matches-list/tournament-matches-list.component';
import { MatCardModule } from '@angular/material/card';
import { GameTypeDto, NotAvailable } from '../tp-model/TpModel';
import { GameTypeService } from '../../Shared/game-type.service';
import { getDateAndTimeStringInFormat } from '../../Shared/Utility/dateTimeUtility';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-tournament-match',
  standalone: true,
  imports: [MatButtonModule,MatCardModule,MatIconModule],
  templateUrl: './tournament-match.component.html',
  styleUrl: './tournament-match.component.scss'
})
export class TournamentMatchComponent {
  @Input({ required: true }) public match?: MatchModel;
  @Input({ required: true }) public gameType?: GameTypeDto | null;
  @Input() public manage: boolean = false;
  @Output() public matchSelectedEE = new EventEmitter<number>();

  private _gameTypeService = inject(GameTypeService);

  public getFirstPlayerName() {
    return this.match?.firstPlayer?.name;
  }

  public getSecondPlayerName() {
    return this.match?.secondPlayer?.name;
  }

  public showAddScore(): boolean
  {
    return this.manage;
  }


  public getMatchScore(): string {
    if (this.match?.winner == null || this.gameType == null || this.match.scoreJson == null) return NotAvailable;
    var scoreString = this._gameTypeService.getFullDisplayeScore(this.gameType, this.match.scoreJson);
    return scoreString;
  }

  public isFirstPlayerWinner(){
    if (this.match?.winner == null || this.gameType == null || this.match.scoreJson == null) return false;
    if(this.match.winner.id == this.match.firstPlayer?.id)return true;
    return false;
  }
  public isSecondPlayerWinner(){
    if (this.match?.winner == null || this.gameType == null || this.match.scoreJson == null) return false;
    if(this.match.winner.id == this.match.secondPlayer?.id)return true;
    return false;
  }

  getMatchCourtName(): string {
    if(this.match?.court == null)return NotAvailable;
    return this.match.court;
  }
  getMatchPlayedDate() {
    if(this.match?.matchPlayed == null)return NotAvailable;
    return getDateAndTimeStringInFormat(new Date(this.match.matchPlayed));
  }

  getMatchScheduledDate() {
    if(this.match?.matchScheduled == null)return NotAvailable;
    return getDateAndTimeStringInFormat(new Date(this.match.matchScheduled));
  }

  public isGamePlayed():boolean{
    if(this.match?.winner)return true;
    return false;
  }
  public addScoreTabChangeView()
  {
    this.matchSelectedEE.emit(this.match?.matchId);
  }
}
