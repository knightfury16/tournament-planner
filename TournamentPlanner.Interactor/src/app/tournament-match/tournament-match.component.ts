import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { MatchModel } from '../tournament-matches-list/tournament-matches-list.component';
import { MatCardModule } from '@angular/material/card';
import { GameTypeDto, NotAvailable } from '../tp-model/TpModel';
import { GameTypeService } from '../../Shared/game-type.service';
import { getDateAndTimeStringInFormat } from '../../Shared/Utility/dateTimeUtility';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-tournament-match',
  standalone: true,
  imports: [MatButtonModule,MatCardModule,MatIconModule, MatChipsModule, MatTooltipModule],
  templateUrl: './tournament-match.component.html',
  styleUrl: './tournament-match.component.scss'
})
export class TournamentMatchComponent {
  @Input({ required: true }) public match?: MatchModel;
  @Input({ required: true }) public gameType?: GameTypeDto | null;
  @Input() public manage: boolean = false;
  @Output() public matchSelectedEE = new EventEmitter<number>();
  public notAvailable = NotAvailable;

  private _gameTypeService = inject(GameTypeService);

  getFirstPlayerName() {
    return this.match?.firstPlayer?.name || 'TBD';
  }

  getSecondPlayerName() {
    return this.match?.secondPlayer?.name || 'TBD';
  }

  showAddScore(): boolean {
    return this.manage;
  }

  getMatchScore(): string {
    if (this.match?.winner == null || this.gameType == null || this.match.scoreJson == null) 
      return NotAvailable;
    return this._gameTypeService.getFullDisplayeScore(this.gameType, this.match.scoreJson);
  }

  isFirstPlayerWinner() {
    if (this.match?.winner == null || this.gameType == null || this.match.scoreJson == null) 
      return false;
    return this.match.winner.id === this.match.firstPlayer?.id;
  }

  isSecondPlayerWinner() {
    if (this.match?.winner == null || this.gameType == null || this.match.scoreJson == null) 
      return false;
    return this.match.winner.id === this.match.secondPlayer?.id;
  }

  getMatchCourtName(): string {
    return this.match?.court || this.notAvailable;
  }

  getMatchPlayedDate() {
    if (!this.match?.matchPlayed) return NotAvailable;
    return getDateAndTimeStringInFormat(new Date(this.match.matchPlayed));
  }

  getMatchScheduledDate() {
    if (!this.match?.matchScheduled) return NotAvailable;
    return getDateAndTimeStringInFormat(new Date(this.match.matchScheduled));
  }

  isGamePlayed(): boolean {
    return !!this.match?.winner;
  }

  addScoreTabChangeView() {
    this.matchSelectedEE.emit(this.match?.matchId);
  }

  getMatchStatus(): { text: string; color: string } {
    if (this.isGamePlayed()) {
      return { text: 'Completed', color: 'accent' };
    }
    if (this.match?.matchScheduled) {
      const now = new Date();
      const scheduled = new Date(this.match.matchScheduled);
      if (scheduled < now) {
        return { text: 'In Progress', color: 'primary' };
      }
      return { text: 'Scheduled', color: 'warn' };
    }
    return { text: 'Pending', color: 'default' };
  }

  hasValidScore(): boolean {
    return this.match?.scoreJson != null && this.getMatchScore() !== NotAvailable;
  }
}
