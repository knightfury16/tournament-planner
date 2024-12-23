import { Component, Input } from '@angular/core';
import { GameTypeSupported, TournamentDto } from '../tp-model/TpModel';
import { transformTournamentIsoDate } from '../../Shared/Utility/dateTimeUtility';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { mapStringToEnum } from '../../Shared/Utility/stringUtility';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-tournament-card',
  standalone: true,
  imports: [MatCardModule, MatChipsModule, RouterModule],
  templateUrl: './tournament-card.component.html',
  styleUrl: './tournament-card.component.scss'
})
export class TournamentCardComponent {

  @Input({ required: true, transform: transformTournamentIsoDate }) tournament: TournamentDto | null = null;

  public getVenue() {
    return this.tournament?.venue ?? 'N/A'
  }

  public getGameType(): GameTypeSupported | null {
    var gameType = this.tournament?.gameTypeDto?.name;
    var gameTypeSupport = mapStringToEnum(GameTypeSupported, gameType!);
    if (gameType) return gameTypeSupport;

    return null;
  }


}
