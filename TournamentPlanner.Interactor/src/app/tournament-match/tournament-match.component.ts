import { Component, inject, Input } from '@angular/core';
import { MatchModel } from '../tournament-matches-list/tournament-matches-list.component';
import { MatCardModule } from '@angular/material/card';
import { GameTypeDto, NotAvailable } from '../tp-model/TpModel';
import { GameTypeService } from '../../Shared/game-type.service';
import { getDateAndTimeStringInFormat } from '../../Shared/Utility/dateTimeUtility';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-tournament-match',
  standalone: true,
  imports: [MatCardModule,MatIconModule],
  templateUrl: './tournament-match.component.html',
  styleUrl: './tournament-match.component.scss'
})
export class TournamentMatchComponent {
  @Input({ required: true }) public match?: MatchModel;
  @Input({ required: true }) public gameType?: GameTypeDto | null;

  private _gameTypeService = inject(GameTypeService);

  public getFirstPlayerName() {
    if (this.match?.winner != null && this.match.winner.id == this.match.firstPlayer?.id) return this.getPlayerNameWithWinnerIcon(this.match.firstPlayer.name);
    return this.match?.firstPlayer?.name;
  }

  public getSecondPlayerName() {
    if (this.match?.winner != null && this.match.winner.id == this.match.secondPlayer?.id) return this.getPlayerNameWithWinnerIcon(this.match.secondPlayer.name);
    return this.match?.secondPlayer?.name;
  }

  getPlayerNameWithWinnerIcon(name: string) {
    return `Winner - name`
  }

  public getMatchScore(): string {
    if (this.match?.winner == null || this.gameType == null || this.match.scoreJson == null) return NotAvailable;
    var scoreString = this._gameTypeService.getFullDisplayeScore(this.gameType, this.match.scoreJson);
    return scoreString;
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
}
