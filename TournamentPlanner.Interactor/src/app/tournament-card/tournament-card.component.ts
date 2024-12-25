import { Component, Input } from '@angular/core';
import { GameTypeSupported, TournamentDto } from '../tp-model/TpModel';
import { transformTournamentIsoDate } from '../../Shared/Utility/dateTimeUtility';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { mapStringToEnum } from '../../Shared/Utility/stringUtility';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tournament-card',
  standalone: true,
  imports: [MatIconModule, MatCardModule, MatButtonModule, MatChipsModule, RouterModule, CommonModule],
  templateUrl: './tournament-card.component.html',
  styleUrl: './tournament-card.component.scss'
})
export class TournamentCardComponent {

  @Input({ required: true, transform: transformTournamentIsoDate }) tournament: TournamentDto | null = null;
  @Input() manage: boolean = false;

  public getVenue() {
    return this.tournament?.venue ?? 'N/A'
  }

  public getGameType(): GameTypeSupported | null {
    var gameType = this.tournament?.gameTypeDto?.name;
    var gameTypeSupport = mapStringToEnum(GameTypeSupported, gameType!);
    if (gameType) return gameTypeSupport;

    return null;
  }

  public getLink(tournamentId: number | undefined): string | any[] | null | undefined {
    if (tournamentId == null) return;
    if (this.manage) return ['/tp/manage-tournament-homepage', tournamentId];

    return ['/tp/tournament-details-homepage', tournamentId];
  }

  public getLinkDiableValue(): boolean {
    //will disable the link from admin view
    return this.manage;
  }

}
